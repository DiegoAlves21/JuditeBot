using DAO;
using DAO.BBL;
using DAO.MapperManual;
using Microsoft.CSharp.RuntimeBinder;
using Model;
using Model.Procucts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Web.Http;
using System.Web.Http.Results;

namespace JuditeBot.Controllers
{
    [RoutePrefix("JuditeBot")]
    public class CadastroController : ApiController
    {

        [Route("signup")]
        [HttpPost]
        public JsonResult<dynamic> signup()
        {
            try
            {
                /*Pizzaria pizzaria = new Pizzaria();
                PizzariaRepositorio repositorio = new PizzariaRepositorio();

                //Incluindo usuários
                IList<Users> usuarios = new List<Users>();
                usuarios.Add(new Users { username = "diego", password = "123" });
                usuarios.Add(new Users { username = "victor", password = "123" });
                usuarios.Add(new Users { username = "leo", password = "123" });
                pizzaria.users = usuarios;

                //Incluindo métodos de pagamento
                IList<PaymentMethod> metodosPagamento = new List<PaymentMethod>();
                metodosPagamento.Add(new PaymentMethod { description = "Débito" });
                metodosPagamento.Add(new PaymentMethod { description = "Crédito" });
                metodosPagamento.Add(new PaymentMethod { description = "Dinheiro" });
                pizzaria.paymentMethods = metodosPagamento;

                //Incluindo produtos
                IList<Product> menus = new List<Product>();
                Pizza pizza = new Pizza { name = "Muzzarela", avaible = true };
                //Pizza pizzaCalabreza = new Pizza { name = "Calabreza", avaible = true };
                Beverage bebida = new Beverage { name = "Coca Cola", avaible = true } ;
                List<ProductInstanceEntity> productInstancePizza = new List<ProductInstanceEntity>();
                List<ProductInstanceEntity> productInstanceBebida = new List<ProductInstanceEntity>();
                ProductSize productSizeBig = new ProductSize { name = "Grande" };
                ProductSize productSizeMedium = new ProductSize { name = "Médio" };
                ProductSize productSizeBebida = new ProductSize { name = "2 litros" };
                productInstancePizza.Add(new ProductInstanceEntity { cost = 30.00, Type = pizza.GetType().Name, productSize = productSizeBig });
                productInstancePizza.Add(new ProductInstanceEntity { cost = 20.00, Type = pizza.GetType().Name, productSize = productSizeMedium });
                productInstanceBebida.Add(new ProductInstanceEntity { cost = 6.00, Type = bebida.GetType().Name, productSize = productSizeBebida });
                bebida.productInstance = productInstanceBebida;
                pizza.productInstance = productInstancePizza;
                //pizzaCalabreza.productInstance = productInstancePizza;
                menus.Add((Product)pizza);
                //menus.Add((Product)pizzaCalabreza);
                menus.Add((Product)bebida);
                pizzaria.menus = menus;

                pizzaria.name = "Pizzaria teste";
                pizzaria.deliveryTax = 10.00;

                repositorio.AdicionarBBL(pizzaria);*/

                PedidoRepositorio pedidoRepositorio = new PedidoRepositorio();


                dynamic retorno = new { sucesso = true, msg = "" };
                return Json(retorno);

            }
            catch(Exception e)
            {
                dynamic retorno = new { sucesso = false, msg = e.Message };
                return Json(retorno);
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
