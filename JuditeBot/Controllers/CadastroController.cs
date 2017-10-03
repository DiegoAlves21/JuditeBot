using DAO;
using DAO.BBL;
using DAO.MapperManual;
using Microsoft.Bot.Connector;
using Microsoft.CSharp.RuntimeBinder;
using Model;
using Model.Enum;
using Model.Procucts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace JuditeBot.Controllers
{
    [RoutePrefix("JuditeBot")]
    public class CadastroController : ApiController
    {

        [Route("signup")]
        [HttpPost]
        public IHttpActionResult signup()
        //public virtual async Task<IHttpActionResult> signup()
        {
            try
            {
                Pizzaria pizzaria = new Pizzaria();
                PizzariaRepositorio repositorio = new PizzariaRepositorio();

                //Incluindo usuários
                IList<Users> usuarios = new List<Users>();
                usuarios.Add(new Users { username = "diego", password = "123" });
                usuarios.Add(new Users { username = "victor", password = "123" });
                usuarios.Add(new Users { username = "leo", password = "123" });
                pizzaria.users = usuarios;

                //Incluindo métodos de pagamento
                IList<CPaymentMethod> metodosPagamento = new List<CPaymentMethod> { new CPaymentMethod { paymentMethod = PaymentMethod.CREDIT }, new CPaymentMethod { paymentMethod = PaymentMethod.DEBIT }, new CPaymentMethod { paymentMethod = PaymentMethod.SODEXO }, new CPaymentMethod { paymentMethod = PaymentMethod.TR } };
                pizzaria.paymentMethods = metodosPagamento;

                //Incluindo produtos
                IList<Product> menus = new List<Product>();

                Product pizza = new Product { name = "Muzzarela", productType = ProductType.PIZZA };
                Product pizzaCalabresa = new Product { name = "Calabresa", productType = ProductType.PIZZA };
                Product pizzaQuatroQueijos = new Product { name = "Quatro Queijos", productType = ProductType.PIZZA };
                Product pizzaPeperoni = new Product { name = "Peperoni", productType = ProductType.PIZZA };
                Product bebida = new Product { name = "Coca Cola", productType = ProductType.BEVERAGE };
                Product bebidaDelVale = new Product { name = "Del Vale Uva", productType = ProductType.BEVERAGE };
                Product bebidaH2oh = new Product { name = "H2oh", productType = ProductType.BEVERAGE };

                List<ProductInstance> productInstancePizza = new List<ProductInstance>();
                List<ProductInstance> productInstancePizzaCalabresa = new List<ProductInstance>();
                List<ProductInstance> productInstancePizzaQuatroQueijos = new List<ProductInstance>();
                List<ProductInstance> productInstancePizzaPeperoni = new List<ProductInstance>();
                List<ProductInstance> productInstanceBebida = new List<ProductInstance>();
                List<ProductInstance> productInstanceBebidaDelVale = new List<ProductInstance>();
                List<ProductInstance> productInstanceBebidaH2oh = new List<ProductInstance>();

                //ProductSize productSizeBig = new ProductSize { name = "Grande" };
                //ProductSize productSizeMedium = new ProductSize { name = "Médio" };
                //ProductSize productSizeBebida = new ProductSize { name = "2 litros" };

                productInstancePizza.Add(new ProductInstance { cost = 30.00, productSize = new ProductSize { name = "Grande" }, available = true });
                productInstancePizza.Add(new ProductInstance { cost = 20.00, productSize = new ProductSize { name = "Medio" }, available = true });
                productInstancePizza.Add(new ProductInstance { cost = 10.00, productSize = new ProductSize { name = "Pequeno" }, available = true });
                //
                productInstancePizzaCalabresa.Add(new ProductInstance { cost = 25.00, productSize = new ProductSize { name = "Grande" }, available = true });
                productInstancePizzaCalabresa.Add(new ProductInstance { cost = 15.00, productSize = new ProductSize { name = "Medio" }, available = true });
                productInstancePizzaCalabresa.Add(new ProductInstance { cost = 8.00, productSize = new ProductSize { name = "Pequeno" }, available = true });
                //
                productInstancePizzaQuatroQueijos.Add(new ProductInstance { cost = 35.00, productSize = new ProductSize { name = "Grande" }, available = true });
                productInstancePizzaQuatroQueijos.Add(new ProductInstance { cost = 25.00, productSize = new ProductSize { name = "Medio" }, available = true });
                productInstancePizzaQuatroQueijos.Add(new ProductInstance { cost = 15.00, productSize = new ProductSize { name = "Pequeno" }, available = true });
                //
                productInstancePizzaPeperoni.Add(new ProductInstance { cost = 30.00, productSize = new ProductSize { name = "Grande" }, available = true });
                productInstancePizzaPeperoni.Add(new ProductInstance { cost = 20.00, productSize = new ProductSize { name = "Medio" }, available = true });
                productInstancePizzaPeperoni.Add(new ProductInstance { cost = 10.00, productSize = new ProductSize { name = "Pequeno" }, available = true });
                //
                productInstanceBebida.Add(new ProductInstance { cost = 6.00, productSize = new ProductSize { name = "2 litros" }, available = true });
                productInstanceBebida.Add(new ProductInstance { cost = 2.00, productSize = new ProductSize { name = "500 ml" }, available = true });
                productInstanceBebida.Add(new ProductInstance { cost = 4.00, productSize = new ProductSize { name = "1 litro" }, available = true });
                //
                productInstanceBebidaDelVale.Add(new ProductInstance { cost = 5.00, productSize = new ProductSize { name = "2 litros" }, available = true });
                productInstanceBebidaDelVale.Add(new ProductInstance { cost = 2.00, productSize = new ProductSize { name = "500 ml" }, available = true });
                productInstanceBebidaDelVale.Add(new ProductInstance { cost = 3.00, productSize = new ProductSize { name = "1 litro" }, available = true });
                //
                productInstanceBebidaH2oh.Add(new ProductInstance { cost = 5.00, productSize = new ProductSize { name = "2 litros" }, available = true });
                productInstanceBebidaH2oh.Add(new ProductInstance { cost = 2.00, productSize = new ProductSize { name = "500 ml" }, available = true });
                productInstanceBebidaH2oh.Add(new ProductInstance { cost = 3.00, productSize = new ProductSize { name = "1 litro" }, available = true });
                //
                bebida.productInstance = productInstanceBebida;
                bebidaDelVale.productInstance = productInstanceBebidaDelVale;
                bebidaH2oh.productInstance = productInstanceBebidaH2oh;
                //
                pizza.productInstance = productInstancePizza;
                pizzaCalabresa.productInstance = productInstancePizzaCalabresa;
                pizzaPeperoni.productInstance = productInstancePizzaPeperoni;
                pizzaQuatroQueijos.productInstance = productInstancePizzaQuatroQueijos;
                //
                menus.Add(pizza);
                menus.Add(pizzaCalabresa);
                menus.Add(pizzaPeperoni);
                menus.Add(pizzaQuatroQueijos);
                //
                menus.Add(bebida);
                menus.Add(bebidaDelVale);
                menus.Add(bebidaH2oh);
                //
                pizzaria.menus = menus;
                pizzaria.name = "Pizzaria teste";
                pizzaria.deliveryTax = 10.00;

                repositorio.AdicionarBBL(pizzaria);

                //PedidoRepositorio pedidoRepositorio = new PedidoRepositorio();
                //Order order = new Order();
                //order.Address = "Endereço Mixed Pizza";
                //order.clientName = "Teste Mixed Pizza";
                //order.created = DateTime.Now;
                //using (var repositorio = new CPaymentMethodRepositorio())
                //{
                //    order.paymentMethodId = repositorio.GetAll().ToList<CPaymentMethod>()[0].Id;
                //}

                ////using (var repositorio = new ProductInstanceRepositorio())
                ////{
                ////    var product = repositorio.GetAll().Take(1).ToList<ProductInstance>();
                ////    order.productInstances = product;
                ////}

                //using (var repositorio = new ProductInstanceRepositorio())
                //{
                //    var product = repositorio.GetAll().Take(2).ToList<ProductInstance>();
                //    order.mixedPizzas = new List<MixedPizza>();
                //    MixedPizza mixed = new MixedPizza();
                //    mixed.productInstances = product;
                //    order.mixedPizzas.Add(mixed);
                //}

                //order.ordersStatus = OrderStatus.WAITING;
                //order.pizzariaId = 1;

                //pedidoRepositorio.AdicionarBBL(order);


                //PizzariaRepositorio pizzariaRepositorio = new PizzariaRepositorio();

                //using (var repositorio = new PizzariaRepositorio())
                //{
                //    var pizzaria = repositorio.GetAll();
                //}

                //var pizzaria = repositorio.GetBBL(p => p.PizzariaId == 1);

                //HttpResponseMessage x = await SendMessageToUser();
                //return new HttpResponseMessage(x.StatusCode);
                return Ok();

            }
            catch(Exception e)
            {
                //return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                return NotFound();
            }
        }
        /*[Route("signup")]
        [HttpPost]
        public JsonResult<object> signup([FromBody]Pizzaria pizzaria)
        {
            //Retorno resultado = new Retorno();
            dynamic resultado;
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                if (pizzaria != null)
                {
                    if (pizzaria.users != null && !string.IsNullOrEmpty(pizzaria.name))
                    {
                        //repositorio.Adicionar(pizzaria);
                        //repositorio.SalvarTodos();
                        //repositorio.Dispose();
                        //resultado = new Retorno() { msgRetorno = "Operação Realizada com sucesso" };

                        var retorno = new PizzariaRepositorio().AdicionarBBL(pizzaria);
                        
                        if(Utils.GetProperty(retorno, "temErro") == true)
                        {
                            resultado = new { msgRetorno = Utils.GetProperty(retorno, "mensagem") };
                            return Json(resultado);
                        }
                        else
                        {
                            resultado = new { msgRetorno = "Operação Realizada com sucesso" };
                        }
                            
                        response.StatusCode = HttpStatusCode.OK;

                        this.ResponseMessage(response);// .CreateResponse(HttpStatusCode.Created, "Criado novo usuário e pizzaria");

                        return Json(resultado);

                    }
                    else
                    {
                        //this.Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro ao tentar criar");
                        response.StatusCode = HttpStatusCode.Unauthorized;
                        this.ResponseMessage(response);
                        resultado = new { msgRetorno = "Parâmetro do usuario recebido parece não estar certo, valor: " + pizzaria };
                        return Json(resultado);
                    }

                }
                else
                {
                    //this.Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro ao tentar criar");
                    response.StatusCode = HttpStatusCode.Unauthorized;
                    this.ResponseMessage(response);
                    resultado = new { msgRetorno = "Parâmetro recebido parece não estar certo, valor: " + pizzaria };
                    return Json(resultado);
                }

            }
            catch (Exception e)
            {
                //this.Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro ao tentar criar");
                response.StatusCode = HttpStatusCode.InternalServerError;
                this.ResponseMessage(response);
                resultado = new { msgRetorno = e.Message };
                return Json(resultado);
            }

        }*/

            //[Route("signin")]
            //[HttpPost]
            //public JsonResult<Retorno> signin([FromBody]Usuario usuario)
            //{
            //    Retorno resultado = new Retorno();
            //    Usuario user = new Usuario();
            //    Pizzaria pizzaria = new Pizzaria();

            //    try
            //     {
            //        using (var repositorio = new UsuarioRepositorio())
            //        {
            //            if (usuario != null)
            //            {
            //                if (usuario.username != null && usuario.password != null)
            //                {
            //                    user = repositorio.Get(u => (u.username == usuario.username && u.password == usuario.password)).ToList().SingleOrDefault();
            //                    repositorio.Dispose();
            //                }
            //                else
            //                {
            //                    this.Request.CreateResponse(HttpStatusCode.Unauthorized, "Login falhou, não autorizado");
            //                    resultado = new Retorno() { msgRetorno = "Parâmetro do usuario recebido parece não estar certo, valor: " + usuario };
            //                    return Json(resultado);
            //                }

            //            }
            //            else
            //            {
            //                this.Request.CreateResponse(HttpStatusCode.Unauthorized, "Login falhou, não autorizado");
            //                resultado = new Retorno() { msgRetorno = "Parâmetro recebido parece não estar certo, valor: " + usuario };
            //                return Json(resultado);
            //            }

            //        }

            //        if (user != null)
            //        {
            //            pizzaria = buscaPizzaria(user.pizzariaId);

            //            if(pizzaria == null)
            //            {
            //                this.Request.CreateResponse(HttpStatusCode.Unauthorized, "Login falhou, não autorizado");
            //                resultado = new Retorno() { msgRetorno = "Erro ao buscar a pizzaria" };
            //                return Json(resultado);
            //            }
            //            else
            //            {
            //                resultado = new Retorno() { msgRetorno = "Operação Realizada com sucesso", entidade = pizzaria };
            //                this.Request.CreateResponse(HttpStatusCode.OK, "Login com sucesso");

            //                return Json(resultado);
            //            }

            //        }
            //        else
            //        {
            //            this.Request.CreateResponse(HttpStatusCode.Unauthorized, "Login falhou, não autorizado");
            //            resultado = new Retorno() { msgRetorno = "Parâmetro do usuario recebido parece não estar certo, valor: " + usuario };
            //            return Json(resultado);
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        this.Request.CreateResponse(HttpStatusCode.Unauthorized, "Login falhou, não autorizado");
            //        resultado = new Retorno() { msgRetorno = e.Message };
            //        return Json(resultado);
            //    }

            //}

        private Pizzaria buscaPizzaria(int id)
        {
            Pizzaria pizzaria = new Pizzaria();
            try
            {
                using (var repositorio = new PizzariaRepositorio())
                {
                    var pizzariaGet = (Pizzaria)repositorio.Get(p => p.PizzariaId == id).ToList().SingleOrDefault();
                    pizzaria = MapperManual.MontaModel(pizzariaGet);
                    repositorio.Dispose();
                    return pizzaria;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public virtual async Task<HttpResponseMessage> SendMessageToUser()
        {
            try
            {
                string toId = "1193748407372016";
                string toName = "Diego Alves";
                string fromId = "166384633836246";
                string fromName = "JuditeBot";
                string serviceUrl = "https://facebook.botframework.com";
                string channelId = "facebook";
                string conversationId = "1193748407372016-166384633836246";

                // Use the data stored previously to create the required objects.
                var userAccount = new ChannelAccount(toId, toName);
                var botAccount = new ChannelAccount(fromId, fromName);
                var connector = new ConnectorClient(new Uri(serviceUrl));

                // Create a new message.
                IMessageActivity message = Activity.CreateMessageActivity();
                if (!string.IsNullOrEmpty(conversationId) && !string.IsNullOrEmpty(channelId))
                {
                    // If conversation ID and channel ID was stored previously, use it.
                    message.ChannelId = channelId;
                }
                else
                {
                    // Conversation ID was not stored previously, so create a conversation. 
                    // Note: If the user has an existing conversation in a channel, this will likely create a new conversation window.
                    conversationId = (await connector.Conversations.CreateDirectConversationAsync(botAccount, userAccount)).Id;
                }

                // Set the address-related properties in the message and send the message.
                message.From = botAccount;
                message.Recipient = userAccount;
                message.Conversation = new ConversationAccount(id: conversationId);
                message.Text = "Hello, this is a notification";
                message.Locale = "en-us";
                MicrosoftAppCredentials.TrustServiceUrl(serviceUrl);
                await connector.Conversations.SendToConversationAsync((Activity)message);
                return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
            }
            catch(Exception e)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }
            
        }

        //private dynamic cadastraCaracteristicasProduto()
        //{
        //    IList<CaracteristicasProduto> caracteristicasProduto = new List<CaracteristicasProduto>();
        //    caracteristicasProduto.Add(new CaracteristicasProduto { tamanho = "Broto", fator = 1 });
        //    caracteristicasProduto.Add(new CaracteristicasProduto { tamanho = "Média", fator = 2 });
        //    caracteristicasProduto.Add(new CaracteristicasProduto { tamanho = "Grande", fator = 3 });
        //    caracteristicasProduto.Add(new CaracteristicasProduto { tamanho = "Família", fator = 4 });
        //    try
        //    {
        //        using (var repositorio = new CaracteristicasProdutoRepositorio())
        //        {
        //            foreach(CaracteristicasProduto c in caracteristicasProduto)
        //            {
        //                repositorio.Adicionar(c);
        //            }
        //            repositorio.SalvarTodos();
        //            repositorio.Dispose();
        //        }
        //        return new { temErro = false, mensagem = "Operação Realizada com Sucesso" };
        //    }
        //    catch (Exception e)
        //    {
        //        return new { temErro = true, mensagem = e.Message };
        //    }
        //}

    }
}
