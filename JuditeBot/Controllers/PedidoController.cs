using DAO.BBL;
using DAO.MapperManual;
using JuditeBot.Model;
using Microsoft.Bot.Connector;
using Microsoft.Owin.Security;
using Model;
using Model.Interfaces;
using Model.Procucts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
        public IHttpActionResult Listar(string from = "", int p = 0, int per_page = 0, string filterPaymentMethod = "", string filterStatus = "", string filterClientName = "", string s = "", string sDir = "")
        {
            //OrdersParam orderParam = new OrdersParam(); 
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
                    var pedidos = FilterDeviceList(pizzaria.orders, from, p, per_page, filterPaymentMethod, filterStatus, filterClientName, s, sDir);
                    //var payment = pizzaria.orders.SingleOrDefault().paymentMethod.paymentMethod;
                    //var status = pizzaria.orders.SingleOrDefault().ordersStatus.ToString();

                    List<OrdersModelView> ordersModelView = new List<OrdersModelView>();

                    foreach (Order pedido in pedidos)
                    {
                        ordersModelView.Add(new OrdersModelView() { address = pedido.Address, clientName = pedido.clientName, createdAt = pedido.created.ToString(), paymentMethod = getPaymentMethod(pedido.paymentMethodId).ToLower(), status = ChangeStatusName(pedido.ordersStatus.ToString()) });
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

        private static IEnumerable<Order> FilterDeviceList(IList<Order> orders, string from, int p, int perPage, string filterPaymentMethod, string filterStatus, string filterClientName, string s, string sDir)
        {
            //var query = orders.AsQueryable();

            var pedidos = orders;

            if (filterClientName != "" && filterClientName != null)
            {
                pedidos = pedidos.Where(ord => ord.clientName.ToUpper() == filterClientName.ToUpper()).ToArray();
                return pedidos;
            }
            else if (filterPaymentMethod != null && filterPaymentMethod != "")
            {
                pedidos = pedidos.Where(ord => ord.paymentMethod.paymentMethod.ToString().ToUpper() == filterPaymentMethod.ToUpper()).ToArray();
                return pedidos;
            }
            else if (filterStatus != null && filterStatus != "")
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
                    ordersModelView.items.mixedPizzas = new List<Products[]>();
                    ordersModelView.items.products = new List<Products>();
                    var order = orderRepositorio.Get(ord => ord.Id == orderId).ToArray().SingleOrDefault();
                    ordersModelView.clientName = order.clientName;
                    ordersModelView.address = order.Address;
                    ordersModelView.status = ChangeStatusName(order.ordersStatus.ToString());
                    using (var repositorio = new CPaymentMethodRepositorio())
                    {
                        foreach (CPaymentMethod pm in repositorio.GetAll().ToList<CPaymentMethod>())
                        {
                            if (order.paymentMethodId == pm.Id)
                            {
                                ordersModelView.paymentMethod = ChangePaymentName(pm.paymentMethod.ToString()).ToLower();
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

                    var contador = 0;

                    foreach (MixedPizza mixedPizza in order.mixedPizzas)
                    {
                        Products[] m = new Products[mixedPizza.productInstances.Count()];
                        contador = 0;

                        foreach (ProductInstance pi in mixedPizza.productInstances)
                        {
                            Products mixedPizzas = new Products();
                            mixedPizzas.name = pi.product.name;
                            mixedPizzas.type = pi.product.productType.ToString();
                            mixedPizzas.size = pi.productSize.name;
                            mixedPizzas.cost = pi.cost.ToString();
                            m[contador] = mixedPizzas;
                            contador++;
                        }
                        ordersModelView.items.mixedPizzas.Add(m);
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
        public virtual async Task<IHttpActionResult> changeStatusPending(int orderId)
        {
            if (this.Authentication.User.Identity.IsAuthenticated)
            {
                try
                {
                    PedidoRepositorio orderRepositorio = new PedidoRepositorio();
                    var order = orderRepositorio.Get(ord => ord.Id == orderId).SingleOrDefault();
                    order.ordersStatus = OrderStatus.WAITING;
                    orderRepositorio.AtualizarBBL(order);
                    HttpResponseMessage x = await SendMessageToUser(order.ordersStatus.ToString(), orderId);
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
        public virtual async Task<IHttpActionResult> changeStatusPreparing(int orderId)
        {
            if (this.Authentication.User.Identity.IsAuthenticated)
            {
                try
                {
                    PedidoRepositorio orderRepositorio = new PedidoRepositorio();
                    var order = orderRepositorio.Get(ord => ord.Id == orderId).SingleOrDefault();
                    order.ordersStatus = OrderStatus.PREPARING;
                    orderRepositorio.AtualizarBBL(order);
                    HttpResponseMessage x = await SendMessageToUser(order.ordersStatus.ToString(), orderId);

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
        public virtual async Task<IHttpActionResult> changeStatusDeliver(int orderId)
        {
            if (this.Authentication.User.Identity.IsAuthenticated)
            {
                try
                {
                    PedidoRepositorio orderRepositorio = new PedidoRepositorio();
                    var order = orderRepositorio.Get(ord => ord.Id == orderId).SingleOrDefault();
                    order.ordersStatus = OrderStatus.OUT_FOR_DELIVERY;
                    orderRepositorio.AtualizarBBL(order);
                    HttpResponseMessage x = await SendMessageToUser(order.ordersStatus.ToString(), orderId);
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
        public virtual async Task<IHttpActionResult> changeStatusFinish(int orderId)
        {
            if (this.Authentication.User.Identity.IsAuthenticated)
            {
                try
                {
                    PedidoRepositorio orderRepositorio = new PedidoRepositorio();
                    var order = orderRepositorio.Get(ord => ord.Id == orderId).SingleOrDefault();
                    order.ordersStatus = OrderStatus.DONE;
                    orderRepositorio.AtualizarBBL(order);
                    HttpResponseMessage x = await SendMessageToUser(order.ordersStatus.ToString(), orderId);
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
        public virtual async Task<IHttpActionResult> changeStatusCancel(int orderId)
        {
            if (this.Authentication.User.Identity.IsAuthenticated)
            {
                try
                {
                    PedidoRepositorio orderRepositorio = new PedidoRepositorio();
                    var order = orderRepositorio.Get(ord => ord.Id == orderId).SingleOrDefault();
                    order.ordersStatus = OrderStatus.CANCELED;
                    orderRepositorio.AtualizarBBL(order);
                    HttpResponseMessage x = await SendMessageToUser(order.ordersStatus.ToString(), orderId);
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

        public virtual async Task<HttpResponseMessage> SendMessageToUser(string newStatus, int orderId)
        {
            try
            {
                //string toId = "1193748407372016";
                //string toName = "Diego Alves";
                //string fromId = "166384633836246";
                //string fromName = "JuditeBot";
                //string serviceUrl = "https://facebook.botframework.com";
                //string channelId = "facebook";
                //string conversationId = "1193748407372016-166384633836246";

                Order order = new Order();
                Client client = new Client();

                using (var orderRepositorio = new PedidoRepositorio())
                {
                    order = orderRepositorio.Get(ord => ord.Id == orderId).SingleOrDefault();
                }

                using (var repositorio = new ClientRepositorio())
                {
                    client = repositorio.Get(c => c.Id == order.clientId).SingleOrDefault();
                }

                // Use the data stored previously to create the required objects.
                var userAccount = new ChannelAccount(client.toId, client.toName);
                var botAccount = new ChannelAccount(client.fromId, client.fromName);
                var connector = new ConnectorClient(new Uri(client.serviceUrl));

                // Create a new message.
                IMessageActivity message = Activity.CreateMessageActivity();
                if (!string.IsNullOrEmpty(client.conversationId) && !string.IsNullOrEmpty(client.channelId))
                {
                    // If conversation ID and channel ID was stored previously, use it.
                    message.ChannelId = client.channelId;
                }
                else
                {
                    // Conversation ID was not stored previously, so create a conversation. 
                    // Note: If the user has an existing conversation in a channel, this will likely create a new conversation window.
                    client.conversationId = (await connector.Conversations.CreateDirectConversationAsync(botAccount, userAccount)).Id;
                }

                // Set the address-related properties in the message and send the message.
                message.From = botAccount;
                message.Recipient = userAccount;
                message.Conversation = new ConversationAccount(id: client.conversationId);
                message.Text = ChangeStatusName(newStatus);
                message.Locale = "en-us";
                MicrosoftAppCredentials.TrustServiceUrl(client.serviceUrl);
                await connector.Conversations.SendToConversationAsync((Activity)message);
                return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
            }
            catch (Exception e)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
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

        private string getPaymentMethod(int id)
        {
            CPaymentMethodRepositorio repositorio = new CPaymentMethodRepositorio();
            var payment = repositorio.Get(p => p.Id == id).SingleOrDefault();
            return ChangePaymentName(payment.paymentMethod.ToString());
        }

        private string ChangePaymentName(string name)
        {
            if (name.ToUpper() == "CREDIT")
            {
                return "CREDITO";
            }
            else if (name.ToUpper() == "DEBIT")
            {
                return "DEBITO";
            }
            else
            {
                return name;
            }
        }

        private string ChangeStatusName(string name)
        {
            if (name.ToUpper() == "WAITING")
            {
                return "Pendente";
            }
            else if (name.ToUpper() == "PREPARING")
            {
                return "Em preparo";
            }
            else if (name.ToUpper() == "OUT_FOR_DELIVERY")
            {
                return "Saiu para entrega";
            }
            else if (name.ToUpper() == "DONE")
            {
                return "Finalizado";
            }
            else
            {
                return "Cancelado";
            }
        }

    }
}
