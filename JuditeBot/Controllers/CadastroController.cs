using DAO.BBL;
using DAO.MapperManual;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace JuditeBot.Controllers
{
    [RoutePrefix("JuditeBot")]
    public class CadastroController : ApiController
    {
        [Route("signup")]
        [HttpPost]
        public JsonResult<Retorno> signup([FromBody]Pizzaria pizzaria)
        {
            Retorno resultado = new Retorno();
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                using (var repositorio = new PizzariaRepositorio())
                {
                    if (pizzaria != null)
                    {
                        if (pizzaria.usuarios != null && !string.IsNullOrEmpty(pizzaria.nome))
                        {
                            repositorio.Adicionar(pizzaria);
                            repositorio.SalvarTodos();

                            resultado = new Retorno() { msgRetorno = "Operação Realizada com sucesso" };
                            
                            response.StatusCode = HttpStatusCode.OK;

                            this.ResponseMessage(response);// .CreateResponse(HttpStatusCode.Created, "Criado novo usuário e pizzaria");

                            return Json(resultado);

                        }
                        else
                        {
                            //this.Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro ao tentar criar");
                            response.StatusCode = HttpStatusCode.Unauthorized;
                            this.ResponseMessage(response);
                            resultado = new Retorno() { msgRetorno = "Parâmetro do usuario recebido parece não estar certo, valor: " + pizzaria };
                            return Json(resultado);
                        }

                    }
                    else
                    {
                        //this.Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro ao tentar criar");
                        response.StatusCode = HttpStatusCode.Unauthorized;
                        this.ResponseMessage(response);
                        resultado = new Retorno() { msgRetorno = "Parâmetro recebido parece não estar certo, valor: " + pizzaria };
                        return Json(resultado);
                    }

                }
            }
            catch (Exception e)
            {
                //this.Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro ao tentar criar");
                response.StatusCode = HttpStatusCode.InternalServerError;
                this.ResponseMessage(response);
                resultado = new Retorno() { msgRetorno = e.Message };
                return Json(resultado);
            }

        }

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
                    var pizzariaGet = (Pizzaria)repositorio.Get(p => p.Id == id).ToList().SingleOrDefault();
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

    }
}
