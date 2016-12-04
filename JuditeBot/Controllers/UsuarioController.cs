using DAO.BBL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;

namespace JuditeBot.Controllers
{
    [RoutePrefix("JuditeBot/Usuario")]
    public class UsuarioController : ApiController
    {
        [Route("Adicionar")]
        [HttpPost]
        public JsonResult<Retorno> Adicionar([FromBody]Usuario usuario)
        {
            Retorno resultado = new Retorno();

            try
            {
                using (var repositorio = new UsuarioRepositorio())
                {
                    if (usuario == null)
                    {
                        resultado = new Retorno() { codRetorno = "1", msgRetorno = "Parâmetro recebido parece não estar certo, valor: " + usuario };
                    }
                    repositorio.Adicionar(usuario);
                    repositorio.SalvarTodos();
                    resultado = new Retorno() { codRetorno = "0", msgRetorno = "Operação Realizada com sucesso"};

                    return Json(resultado);
                }
            }
            catch(Exception e)
            {
                resultado = new Retorno() { codRetorno = "99", msgRetorno = e.Message };
                return Json(resultado);
            }

        }

        [Route("Buscar/{id}")]
        [HttpGet]
        public JsonResult<Retorno> Buscar(int? id)
        {
            Retorno resultado = new Retorno();

            try
            {
                using (var repositorio = new UsuarioRepositorio())
                {
                    if(id == null)
                    {
                        resultado = new Retorno() { codRetorno = "1", msgRetorno = "Parâmetro recebido parece não estar certo, valor: " + id };

                        return Json(resultado);
                    }
                    Usuario usuario = repositorio.Find(id);
                    resultado = new Retorno() { codRetorno = "0", msgRetorno = "Operação Realizada com sucesso", entidade = usuario };

                    return Json(resultado);
                }
            }
            catch (Exception e)
            {
                resultado = new Retorno() { codRetorno = "99", msgRetorno = e.Message };
                return Json(resultado);
            }

        }

        [Route("Buscar/{nome}/{senha}")]
        [HttpPost]
        public JsonResult<Retorno> Buscar(string nome, string senha)
        {
            Retorno resultado = new Retorno();

            try
            {
                using (var repositorio = new UsuarioRepositorio())
                {
                    if (String.IsNullOrEmpty(nome) || String.IsNullOrEmpty(senha))
                    {
                        resultado = new Retorno() { codRetorno = "1", msgRetorno = "Parâmetro recebido parece não estar certo, valor(es): " + nome + " / " + senha };

                        return Json(resultado);
                    }
                    var usuario = repositorio.Get(u => (u.nome == nome && u.senha == senha)).ToList().SingleOrDefault();
                    resultado = new Retorno() { codRetorno = "0", msgRetorno = "Operação Realizada com sucesso", entidade = usuario };

                    return Json(resultado);
                }
            }
            catch (Exception e)
            {
                resultado = new Retorno() { codRetorno = "99", msgRetorno = e.Message };
                return Json(resultado);
            }

        }

        [Route("Excluir")]
        [HttpPost]
        public JsonResult<Retorno> Excluir([FromBody]Usuario usuario)
        {
            Retorno resultado = new Retorno();

            try
            {
                using (var repositorio = new UsuarioRepositorio())
                {
                    if (usuario.Id == 0)
                    {
                        resultado = new Retorno() { codRetorno = "1", msgRetorno = "Parâmetro recebido parece não estar certo, valor(es): " + usuario };

                        return Json(resultado);
                    }
                    repositorio.Excluir(u => u.Id == usuario.Id);
                    repositorio.SalvarTodos();
                    resultado = new Retorno() { codRetorno = "0", msgRetorno = "Exclusão Realizada com sucesso"};

                    return Json(resultado);
                }
            }
            catch (Exception e)
            {
                resultado = new Retorno() { codRetorno = "99", msgRetorno = e.Message };
                return Json(resultado);
            }

        }

        [Route("Atualizar")]
        [HttpPost]
        public JsonResult<Retorno> Atualizar([FromBody]Usuario usuario)
        {
            Retorno resultado = new Retorno();

            try
            {
                using (var repositorio = new UsuarioRepositorio())
                {
                    if (usuario == null)
                    {
                        resultado = new Retorno() { codRetorno = "1", msgRetorno = "Parâmetro recebido parece não estar certo, valor: " + usuario };
                    }
                    repositorio.Atualizar(usuario);
                    repositorio.SalvarTodos();
                    resultado = new Retorno() { codRetorno = "0", msgRetorno = "Operação Realizada com sucesso" };

                    return Json(resultado);
                }
            }
            catch (Exception e)
            {
                resultado = new Retorno() { codRetorno = "99", msgRetorno = e.Message };
                return Json(resultado);
            }

        }
    }
}
