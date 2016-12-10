using DAO.BBL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http.Results;
using System.Web.Http;

namespace JuditeBot.Controllers
{
    [RoutePrefix("JuditeBot/Produto")]
    public class ProdutoController : ApiController
    {
        [Route("Adicionar")]
        [HttpPost]
        public JsonResult<Retorno> Adicionar([FromBody]Produto produto)
        {
            Retorno resultado = new Retorno();

            try
            {
                using (var repositorio = new ProdutoRepositorio())
                {
                    if (produto == null)
                    {
                        resultado = new Retorno() { msgRetorno = "Parâmetro recebido parece não estar certo, valor: " + produto };
                    }
                    repositorio.Adicionar(produto);
                    repositorio.SalvarTodos();
                    resultado = new Retorno() { msgRetorno = "Operação Realizada com sucesso" };

                    return Json(resultado);
                }
            }
            catch (Exception e)
            {
                resultado = new Retorno() { msgRetorno = e.Message };
                return Json(resultado);
            }

        }

        //[Route("Buscar/{id}")]
        //[HttpGet]
        //public JsonResult<Retorno> Buscar(int? id)
        //{
        //    Retorno resultado = new Retorno();

        //    try
        //    {
        //        using (var repositorio = new ProdutoRepositorio())
        //        {
        //            if (id == null)
        //            {
        //                resultado = new Retorno() { codRetorno = "1", msgRetorno = "Parâmetro recebido parece não estar certo, valor: " + id };

        //                return Json(resultado);
        //            }
        //            Produto produto = repositorio.Find(id);
        //            resultado = new Retorno() { codRetorno = "0", msgRetorno = "Operação Realizada com sucesso", entidade = produto };

        //            return Json(resultado);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        resultado = new Retorno() { codRetorno = "99", msgRetorno = e.Message };
        //        return Json(resultado);
        //    }

        //}

        //[Route("Buscar/{nome}")]
        //[HttpPost]
        //public JsonResult<Retorno> Buscar(string nome)
        //{
        //    Retorno resultado = new Retorno();

        //    try
        //    {
        //        using (var repositorio = new ProdutoRepositorio())
        //        {
        //            if (String.IsNullOrEmpty(nome))
        //            {
        //                resultado = new Retorno() { codRetorno = "1", msgRetorno = "Parâmetro recebido parece não estar certo, valor(es): " + nome };

        //                return Json(resultado);
        //            }
        //            var produto = repositorio.Get(u => (u.nome == nome)).ToList().SingleOrDefault();
        //            resultado = new Retorno() { codRetorno = "0", msgRetorno = "Operação Realizada com sucesso", entidade = produto };

        //            return Json(resultado);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        resultado = new Retorno() { codRetorno = "99", msgRetorno = e.Message };
        //        return Json(resultado);
        //    }

        //}

        //[Route("Excluir")]
        //[HttpPost]
        //public JsonResult<Retorno> Excluir([FromBody]Produto produto)
        //{
        //    Retorno resultado = new Retorno();

        //    try
        //    {
        //        using (var repositorio = new ProdutoRepositorio())
        //        {
        //            if (produto.Id == 0)
        //            {
        //                resultado = new Retorno() { codRetorno = "1", msgRetorno = "Parâmetro recebido parece não estar certo, valor(es): " + produto };

        //                return Json(resultado);
        //            }
        //            repositorio.Excluir(u => u.Id == produto.Id);
        //            repositorio.SalvarTodos();
        //            resultado = new Retorno() { codRetorno = "0", msgRetorno = "Exclusão Realizada com sucesso" };

        //            return Json(resultado);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        resultado = new Retorno() { codRetorno = "99", msgRetorno = e.Message };
        //        return Json(resultado);
        //    }

        //}

        //[Route("Atualizar")]
        //[HttpPost]
        //public JsonResult<Retorno> Atualizar([FromBody]Produto produto)
        //{
        //    Retorno resultado = new Retorno();

        //    try
        //    {
        //        using (var repositorio = new ProdutoRepositorio())
        //        {
        //            if (produto == null)
        //            {
        //                resultado = new Retorno() { codRetorno = "1", msgRetorno = "Parâmetro recebido parece não estar certo, valor: " + produto };
        //            }
        //            repositorio.Atualizar(produto);
        //            repositorio.SalvarTodos();
        //            resultado = new Retorno() { codRetorno = "0", msgRetorno = "Operação Realizada com sucesso" };

        //            return Json(resultado);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        resultado = new Retorno() { codRetorno = "99", msgRetorno = e.Message };
        //        return Json(resultado);
        //    }

        //}
    }
}
