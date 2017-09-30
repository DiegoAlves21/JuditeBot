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

        //[Route("orders")]
        //[HttpPost]
        //public IHttpActionResult Cadastrar([FromBody] OrdersViewModel ordersViewModel)
        //{
        //    if (this.Authentication.User.Identity.IsAuthenticated)
        //    {
        //        try
        //        {
        //            Order order = ConvertOrdersViewModel(ordersViewModel);

        //            if (true == true)// Aqui entra as validações
        //            {
        //                PizzariaRepositorio pizzariaRepositorio = new PizzariaRepositorio();
        //                var pizzariaId = int.Parse(this.Authentication.User.Claims.SingleOrDefault().Value);
        //                var pizzaria = pizzariaRepositorio.Get(pi => pi.PizzariaId == pizzariaId).SingleOrDefault();

        //                pizzaria.orders.Add(order);
        //                pizzariaRepositorio.AtualizarBBL(pizzaria);

        //                return Created("Criado", ordersViewModel);
        //            }
        //            else
        //            {
        //                return new System.Web.Http.Results.ResponseMessageResult(
        //                    Request.CreateErrorResponse(
        //                        (HttpStatusCode)422,
        //                        new HttpError("Erros de validação ocorreram")
        //                    )
        //                );
        //            }

        //        }
        //        catch (Exception e)
        //        {
        //            return InternalServerError(e);
        //        }

        //    }
        //    else
        //    {
        //        return Unauthorized();
        //    }

        //}

        [Route("orders")]
        [HttpGet]
        public IHttpActionResult Listar(OrdersParam orderParam)
        {
            //int p = 0;
            //int perPage = 0;
            //string filterPaymentMethod = "CREDIT";
            //string filterStatus = "";
            //string filterClientName = "";
            //string s = "";
            //string sDir = "";

            if (this.Authentication.User.Identity.IsAuthenticated)
            {
                try
                {
                    PizzariaRepositorio pizzariaRepositorio = new PizzariaRepositorio();
                    var pizzariaId = int.Parse(this.Authentication.User.Claims.SingleOrDefault().Value);
                    var pizzaria = pizzariaRepositorio.Get(pi => pi.PizzariaId == pizzariaId).SingleOrDefault();
                    //var pedidos = pizzaria.orders.Where(o => o.paymentMethod.paymentMethod.ToString().ToUpper() == filterPaymentMethod).ToArray().Take(p);
                    var pedidos = FilterDeviceList(pizzaria.orders, orderParam.p, orderParam.per_page, orderParam.filterPaymentMethod, orderParam.filterStatus, orderParam.filterClientName, orderParam.s, orderParam.sDir);
                    //var payment = pizzaria.orders.SingleOrDefault().paymentMethod.paymentMethod;
                    //var status = pizzaria.orders.SingleOrDefault().ordersStatus.ToString();

                    List<OrdersModelView> ordersModelView = new List<OrdersModelView>();

                    foreach (Order pedido in pedidos)
                    {
                        ordersModelView.Add(new OrdersModelView() { address = pedido.Address, clientName = pedido.clientName, createdAt = pedido.created.ToString(), paymentMethod = pedido.paymentMethodId.ToString(), status = pedido.ordersStatus.ToString() });
                    }

                    return Ok(ordersModelView.ToList());
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

        private static IEnumerable<Order> FilterDeviceList(IList<Order> orders, int p, int perPage, string filterPaymentMethod, string filterStatus, string filterClientName, string s, string sDir)
        {
            //var query = orders.AsQueryable();

            var pedidos = orders;

            if (filterClientName != "null" && filterClientName != null)
            {
                pedidos = pedidos.Where(ord => ord.clientName.ToUpper() == filterClientName.ToUpper()).ToArray();
                return pedidos;
            }
            else if (filterPaymentMethod != null && filterPaymentMethod != "null")
            {
                pedidos = pedidos.Where(ord => ord.paymentMethod.paymentMethod.ToString().ToUpper() == filterPaymentMethod.ToUpper()).ToArray();
                return pedidos;
            }
            else if (filterStatus != null && filterStatus != "null")
            {
                pedidos = pedidos.Where(ord => ord.ordersStatus.ToString().ToUpper() == filterStatus.ToUpper()).ToArray();
                return pedidos;
            }
            else if (filterStatus != null && filterStatus != "null")
            {
                pedidos = pedidos.Where(ord => ord.ordersStatus.ToString().ToUpper() == filterStatus.ToUpper()).ToArray();
                return pedidos;
            }
            else
            {
                return pedidos;
            }

            ///VERIFICAR E REFAZER PARA FUNCIONAMENTO CORRETO    
            ///if (filterPaymentMethod != null)
            ///{
            ///    query = query.Where(ord => ord.paymentMethod == filterPaymentMethod);
            ///}
            ///if (filterStatus != null)
            ///{
            ///    query = query.Where(ord => ord.ordersStatus == filterStatus);
            ///}

            /*if (perPage != 0)
            {
                query = query.Take(perPage);
            }*/

            //ordenação
            /*if (sDir != null)
            {
                if (sDir == "ASC")
                {
                    pedidos = pedidos.OrderBy(ord => ord.clientName);
                }
                else
                {
                    pedidos = pedidos.OrderByDescending(ord => ord.clientName);
                }

            }*/
            
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
                        foreach (CPaymentMethod pm in repositorio.GetAll().ToList<CPaymentMethod>())
                        {
                            if (order.paymentMethodId == pm.Id)
                            {
                                ordersModelView.paymentMethod = pm.paymentMethod.ToString();
                            }
                        }
                    }
                    ordersModelView.createdAt = order.created.ToString();

                    foreach (ProductInstance pi in order.productInstances)
                    {
                        Products products = new Products();
                        products.name = pi.product.name;
                        products.type = pi.product.productType.ToString();
                        products.size = pi.productSize.name;
                        products.cost = pi.cost.ToString();
                        ordersModelView.items.products.Add(products);
                    }

                    foreach (MixedPizza mixedPizza in order.mixedPizzas)
                    {
                        foreach (ProductInstance pi in mixedPizza.productInstances)
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

        [Route("orders/{orderId}/preparing")]
        [HttpGet]
        public IHttpActionResult changeStatusPreparing(int orderId)
        {
            if (this.Authentication.User.Identity.IsAuthenticated)
            {
                try
                {
                    PedidoRepositorio orderRepositorio = new PedidoRepositorio();
                    var order = orderRepositorio.Get(ord => ord.Id == orderId).SingleOrDefault();
                    order.ordersStatus = OrderStatus.PREPARING;
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
                        order.productInstances.Add(new ProductInstance { Id = ordersViewModel.flavors[i] });
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
