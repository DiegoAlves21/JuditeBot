using DAO.BBL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http.Results;
using System.Web.Http;
using Microsoft.Owin.Security;
using System.Web;
using System.Net.Http;

namespace JuditeBot.Controllers
{
    [RoutePrefix("JuditeBot")]
    public class ProdutoController : ApiController
    {
        // Retorna Nosso Authentication Manager
        private IAuthenticationManager Authentication
        {
            get { return HttpContext.Current.GetOwinContext().Authentication; }
        }

        [Route("products")]
        [Authorize]
        [HttpPost]
        public JsonResult<dynamic> Adicionar([FromBody]Product produto)
        {
            dynamic resultado;

            var pizzariaId = int.Parse(this.Authentication.User.Claims.SingleOrDefault().Value);

            try
            {
                using (var repositorio = new PizzariaRepositorio())
                {
                    if (produto == null)
                    {
                        resultado = new { msgRetorno = "Parâmetro recebido parece não estar certo, valor: "};
                    }
                    else
                    {
                        var pizzaria = repositorio.Get(p => p.PizzariaId == pizzariaId).SingleOrDefault();

                        if (pizzaria.menus == null)
                        {
                            pizzaria.menus = new List<Product>();
                        }

                        pizzaria.menus.Add(produto);

                        repositorio.Atualizar(pizzaria);

                        repositorio.SalvarTodos();

                        resultado = new { msgRetorno = "Operação Realizada com sucesso" };

                        
                    }
                    

                    return Json(resultado);
                }
            }
            catch (Exception e)
            {
                resultado = new { msgRetorno = e.Message };
                return Json(resultado);
            }

        }

        [Route("products")]
        [Authorize]
        [HttpGet]
        public JsonResult<dynamic> Listar()
        {
            dynamic resultado;
            HttpResponseMessage response = new HttpResponseMessage();
            IList<dynamic> produtos = new List<dynamic>();
            var pizzariaId = int.Parse(this.Authentication.User.Claims.SingleOrDefault().Value);
            
            try
            {
                using (var repositorio = new PizzariaRepositorio())
                {
                    var pizzaria = repositorio.Get(p => p.PizzariaId == pizzariaId).SingleOrDefault();

                    if (pizzaria.menus == null)
                    {
                        response.StatusCode = HttpStatusCode.NotFound;
                        this.ResponseMessage(response);
                        resultado = new { msgRetorno = "Não há produtos cadastrados" };
                    }
                    else
                    {
                        foreach (Product p in pizzaria.menus)
                        {
                            produtos.Add(new { Id = 1, nome = p.name, valor = p.avaible});
                        }
                        //this.NotFound();
                        //response.StatusCode = HttpStatusCode.NotModified;
                        this.ResponseMessage(response);
                        this.ActionContext.Response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };

                        resultado = new { msgRetorno = "Operação Realizada com sucesso", entidade = produtos };
                    }

                    return Json(resultado);
                }
            }
            catch (Exception e)
            {
                resultado = new { msgRetorno = e.Message };
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
