using DAO.BBL;
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

            try
            {
                using (var repositorio = new PizzariaRepositorio())
                {
                    if (pizzaria != null)
                    {
                        if (pizzaria.usuario != null)
                        {
                            repositorio.Adicionar(pizzaria);
                            repositorio.SalvarTodos();

                            var p = repositorio.Get(u => (u.nome == pizzaria.nome)).ToList().SingleOrDefault();

                            resultado = new Retorno() { msgRetorno = "Operação Realizada com sucesso", entidade = p };

                            this.Request.CreateResponse(HttpStatusCode.Created, "Criado novo usuário e pizzaria");

                            return Json(resultado);

                        }
                        else
                        {
                            this.Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro ao tentar criar");
                            resultado = new Retorno() { msgRetorno = "Parâmetro do usuario recebido parece não estar certo, valor: " + pizzaria };
                            return Json(resultado);
                        }

                    }
                    else
                    {
                        this.Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro ao tentar criar");
                        resultado = new Retorno() { msgRetorno = "Parâmetro recebido parece não estar certo, valor: " + pizzaria };
                        return Json(resultado);
                    }

                }
            }
            catch (Exception e)
            {
                this.Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro ao tentar criar");
                resultado = new Retorno() { msgRetorno = e.Message };
                return Json(resultado);
            }

        }

        [Route("signin")]
        [HttpPost]
        public JsonResult<Retorno> signin([FromBody]Usuario usuario)
        {
            Retorno resultado = new Retorno();
            Usuario user = new Usuario();

            try
            {
                using (var repositorio = new UsuarioRepositorio())
                {
                    if (usuario != null)
                    {
                        if (usuario.username != null && usuario.password != null)
                        {
                            user = repositorio.Get(u => (u.username == usuario.username && u.password == usuario.password)).ToList().SingleOrDefault();
                            repositorio.Dispose();
                        }
                        else
                        {
                            this.Request.CreateResponse(HttpStatusCode.Unauthorized, "Login falhou, não autorizado");
                            resultado = new Retorno() { msgRetorno = "Parâmetro do usuario recebido parece não estar certo, valor: " + usuario };
                            return Json(resultado);
                        }

                    }
                    else
                    {
                        this.Request.CreateResponse(HttpStatusCode.Unauthorized, "Login falhou, não autorizado");
                        resultado = new Retorno() { msgRetorno = "Parâmetro recebido parece não estar certo, valor: " + usuario };
                        return Json(resultado);
                    }

                }

                if (user != null)
                {
                    var pizzaria = buscaPizzaria(user.Id);

                    if(pizzaria == null)
                    {
                        this.Request.CreateResponse(HttpStatusCode.Unauthorized, "Login falhou, não autorizado");
                        resultado = new Retorno() { msgRetorno = "Erro ao buscar a pizzaria" };
                        return Json(resultado);
                    }
                    else
                    {
                        resultado = new Retorno() { msgRetorno = "Operação Realizada com sucesso", entidade = pizzaria };
                        this.Request.CreateResponse(HttpStatusCode.OK, "Login com sucesso");

                        return Json(resultado);
                    }
                    
                }
                else
                {
                    this.Request.CreateResponse(HttpStatusCode.Unauthorized, "Login falhou, não autorizado");
                    resultado = new Retorno() { msgRetorno = "Parâmetro do usuario recebido parece não estar certo, valor: " + usuario };
                    return Json(resultado);
                }
            }
            catch (Exception e)
            {
                this.Request.CreateResponse(HttpStatusCode.Unauthorized, "Login falhou, não autorizado");
                resultado = new Retorno() { msgRetorno = e.Message };
                return Json(resultado);
            }

        }

        private Pizzaria buscaPizzaria(int id)
        {
            Pizzaria pizzaria = new Pizzaria();
            try
            {
                using (var repositorio = new PizzariaRepositorio())
                {
                    pizzaria = repositorio.Get(p => p.usuario.Id == id).SingleOrDefault();
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
