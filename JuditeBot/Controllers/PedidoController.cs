using DAO.BBL;
using DAO.MapperManual;
using JuditeBot.Model;
using Microsoft.Owin.Security;
using Model;
using Model.Interfaces;
using Model.Procucts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace JuditeBot.Controllers
{
    [RoutePrefix("JuditeBot")]
    public class PedidoController : ApiController
    {
        // Retorna Nosso Authentication Manager
        private IAuthenticationManager Authentication
        {
            get { return HttpContext.Current.GetOwinContext().Authentication; }
        }

        [Route("orders")]
        [HttpPost]
        public IHttpActionResult Cadastrar([FromBody] OrdersViewModel ordersViewModel)
        {
            if (this.Authentication.User.Identity.IsAuthenticated)
            {
                try
                {
                    Order order = ConvertOrdersViewModel(ordersViewModel);

                    if (true == true)// Aqui entra as validações
                    {
                        PizzariaRepositorio pizzariaRepositorio = new PizzariaRepositorio();
                        var pizzariaId = int.Parse(this.Authentication.User.Claims.SingleOrDefault().Value);
                        var pizzaria = pizzariaRepositorio.Get(pi => pi.PizzariaId == pizzariaId).SingleOrDefault();

                        pizzaria.orders.Add(order);
                        pizzariaRepositorio.AtualizarBBL(pizzaria);

                        return Created("Criado", ordersViewModel);
                    }
                    else
                    {
                        return new System.Web.Http.Results.ResponseMessageResult(
                            Request.CreateErrorResponse(
                                (HttpStatusCode)422,
                                new HttpError("Erros de validação ocorreram")
                            )
                        );
                    }

                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }

            }
            else
            {
                return Unauthorized();
            }

        }


        [Route("orders")]
        [HttpGet]
        public IHttpActionResult Listar()
        {
            if (this.Authentication.User.Identity.IsAuthenticated)
            {
                throw new NotImplementedException();
            }
            else
            {
                return Unauthorized();
            }

        }


        [Route("orders/{orderId}")]
        [HttpGet]
        public IHttpActionResult Find(int orderId)
        {
            if (this.Authentication.User.Identity.IsAuthenticated)
            {
                try
                {
                    OrdersModelView ordersModelView = new OrdersModelView();
                    PedidoRepositorio orderRepositorio = new PedidoRepositorio();
                    ordersModelView.items = new Items();
                    ordersModelView.items.mixedPizzas = new List<Products>();
                    ordersModelView.items.products = new List<Products>();
                    var order = orderRepositorio.Get(ord => ord.Id == orderId).ToArray().SingleOrDefault();
                    ordersModelView.clientName = order.clientName;
                    ordersModelView.address = order.Address;
                    ordersModelView.status = order.ordersStatus.ToString();
                    using (var repositorio = new CPaymentMethodRepositorio())
                    {
                        foreach(CPaymentMethod pm in repositorio.GetAll().ToList<CPaymentMethod>())
                        {
                            if(order.paymentMethodId == pm.Id)
                            {
                                ordersModelView.paymentMethod = pm.paymentMethod.ToString();
                            }
                        }
                    }
                    ordersModelView.createdAt = order.created.ToString();
                    
                    foreach(ProductInstance pi in order.productInstances)
                    {
                        Products products = new Products();
                        products.name = pi.product.name;
                        products.type = pi.product.productType.ToString();
                        products.size = pi.productSize.name;
                        products.cost = pi.cost.ToString();
                        ordersModelView.items.products.Add(products);
                    }

                    foreach(MixedPizza mixedPizza in order.mixedPizzas)
                    {
                        foreach(ProductInstance pi in mixedPizza.productInstances)
                        {
                            Products mixedPizzas = new Products();
                            mixedPizzas.name = pi.product.name;
                            mixedPizzas.type = pi.product.productType.ToString();
                            mixedPizzas.size = pi.productSize.name;
                            mixedPizzas.cost = pi.cost.ToString();
                            ordersModelView.items.mixedPizzas.Add(mixedPizzas);
                        }
                    }
                    
                    return Ok(ordersModelView);
                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }

            }
            else
            {
                return Unauthorized();
            }

        }

        [Route("orders/{orderId}/products")]
        [HttpGet]
        public IHttpActionResult FindProducts(int orderId)
        {
            if (this.Authentication.User.Identity.IsAuthenticated)
            {
                try
                {
                    PedidoRepositorio orderRepositorio = new PedidoRepositorio();
                    var order = orderRepositorio.Get(ord => ord.Id == orderId).ToArray().SingleOrDefault();
                    ProductViewModel productViewModel = new ProductViewModel();
                    productViewModel.instances = new List<Instances>();

                    foreach (ProductInstance p in order.productInstances)
                    {
                        Instances instances = new Instances();
                        productViewModel.name = p.product.name;
                        productViewModel.type = p.product.productType.ToString();
                        instances.size = p.productSize.name;
                        instances.cost = p.cost;
                        instances.available = p.available;
                        productViewModel.instances.Add(instances);
                    }

                    return Ok(productViewModel);
                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }

            }
            else
            {
                return Unauthorized();
            }

        }

        [Route("orders/{orderId}/pending")]
        [HttpGet]
        public IHttpActionResult changeStatusPending(int orderId)
        {
            if (this.Authentication.User.Identity.IsAuthenticated)
            {
                try
                {
                    PedidoRepositorio orderRepositorio = new PedidoRepositorio();
                    var order = orderRepositorio.Get(ord => ord.Id == orderId).SingleOrDefault();
                    order.ordersStatus = OrderStatus.WAITING;
                    orderRepositorio.AtualizarBBL(order);
                    return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)204));
                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }

            }
            else
            {
                return Unauthorized();
            }

        }

        [Route("orders/{orderId}/deliver")]
        [HttpGet]
        public IHttpActionResult changeStatusDeliver(int orderId)
        {
            if (this.Authentication.User.Identity.IsAuthenticated)
            {
                try
                {
                    PedidoRepositorio orderRepositorio = new PedidoRepositorio();
                    var order = orderRepositorio.Get(ord => ord.Id == orderId).SingleOrDefault();
                    order.ordersStatus = OrderStatus.OUT_FOR_DELIVERY;
                    orderRepositorio.AtualizarBBL(order);
                    return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)204));
                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }

            }
            else
            {
                return Unauthorized();
            }

        }

        [Route("orders/{orderId}/finish")]
        [HttpGet]
        public IHttpActionResult changeStatusFinish(int orderId)
        {
            if (this.Authentication.User.Identity.IsAuthenticated)
            {
                try
                {
                    PedidoRepositorio orderRepositorio = new PedidoRepositorio();
                    var order = orderRepositorio.Get(ord => ord.Id == orderId).SingleOrDefault();
                    order.ordersStatus = OrderStatus.DONE;
                    orderRepositorio.AtualizarBBL(order);
                    return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)204));
                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }

            }
            else
            {
                return Unauthorized();
            }

        }

        [Route("orders/{orderId}/cancel")]
        [HttpGet]
        public IHttpActionResult changeStatusCancel(int orderId)
        {
            if (this.Authentication.User.Identity.IsAuthenticated)
            {
                try
                {
                    PedidoRepositorio orderRepositorio = new PedidoRepositorio();
                    var order = orderRepositorio.Get(ord => ord.Id == orderId).SingleOrDefault();
                    order.ordersStatus = OrderStatus.CANCELED;
                    orderRepositorio.AtualizarBBL(order);
                    return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)204));
                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }

            }
            else
            {
                return Unauthorized();
            }

        }

        private Order ConvertOrdersViewModel(OrdersViewModel ordersViewModel)
        {
            if (ordersViewModel != null)
            {
                Order order = new Order();

                order.clientName = ordersViewModel.clientName;
                order.Address = ordersViewModel.Address;
                order.paymentMethod = new CPaymentMethod();

                if (ordersViewModel.paymentMethod.ToUpper() == PaymentMethod.CREDIT.ToString().ToUpper())
                {
                    order.paymentMethod.paymentMethod = PaymentMethod.CREDIT;
                }
                else if (ordersViewModel.paymentMethod.ToUpper() == PaymentMethod.DEBIT.ToString().ToUpper())
                {
                    order.paymentMethod.paymentMethod = PaymentMethod.DEBIT;
                }
                else if (ordersViewModel.paymentMethod.ToUpper() == PaymentMethod.SODEXO.ToString().ToUpper())
                {
                    order.paymentMethod.paymentMethod = PaymentMethod.SODEXO;
                }
                else
                {
                    order.paymentMethod.paymentMethod = PaymentMethod.TR;
                }

                if (ordersViewModel.flavors != null)
                {
                    order.productInstances = new List<ProductInstance>();
                    for (int i = 0; i < ordersViewModel.flavors.Count(); i++)
                    {
                        order.productInstances.Add(new ProductInstance { Id = ordersViewModel.flavors[i]});
                    }
                }
                return order;
            }
            else
            {
                return null;
            }

        }

    }
}
