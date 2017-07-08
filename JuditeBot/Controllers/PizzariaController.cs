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
    public class PizzariaController : ApiController
    {
        //[Route("pizzarias/{id}/products/")]
        //[HttpGet]
        //public JsonResult<Retorno> getProdutos(int? id)
        //{
        //    Retorno resultado = new Retorno();
        //    Pizzaria pizzaria = new Pizzaria();

        //    try
        //    {
        //        using (var repositorio = new PizzariaRepositorio())
        //        {
        //            if (id != null)
        //            {
                       
        //                pizzaria = repositorio.Find(id);

        //                if(pizzaria.produtos == null)
        //                {
        //                    pizzaria.produtos = new List<Produto>();
        //                }

        //                resultado = new Retorno() { msgRetorno = "Operação Realizada com sucesso", entidade = pizzaria.produtos };

        //                this.Request.CreateResponse(HttpStatusCode.OK, "Todos os produtos vinculados a pizzaria");

        //                return Json(resultado);


        //            }
        //            else
        //            {
        //                this.Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro ao tentar criar");
        //                resultado = new Retorno() { msgRetorno = "Parâmetro recebido parece não estar certo, valor: " + id };
        //                return Json(resultado);
        //            }

        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        this.Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro ao tentar buscar os produtos");
        //        resultado = new Retorno() { msgRetorno = e.Message };
        //        return Json(resultado);
        //    }

        //}

        //[Route("pizzarias/{id}/products/")]
        //[HttpPost]
        //public JsonResult<Retorno> CreateProductsInPizzaria(int? id, [FromBody] List<Produto> produtos)
        //{
        //    Retorno resultado = new Retorno();
        //    Pizzaria pizzaria = new Pizzaria();

        //    try
        //    {
        //        using (var repositorio = new PizzariaRepositorio())
        //        {
        //            if (id != null)
        //            {

        //                pizzaria = repositorio.Find(id);
        //                if (pizzaria != null && produtos != null)
        //                {
        //                    pizzaria.produtos = produtos;
        //                    repositorio.Atualizar(pizzaria);
        //                    repositorio.SalvarTodos();
        //                    resultado = new Retorno() { msgRetorno = "Operação Realizada com sucesso", entidade = pizzaria.produtos };

        //                    this.Request.CreateResponse(HttpStatusCode.OK, "Produtos criados com sucesso");

        //                    return Json(resultado);
        //                }
        //                else
        //                {
        //                    this.Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro ao tentar criar");
        //                    resultado = new Retorno() { msgRetorno = "Pizzaria não encontrada para armazenar os produtos, valor: " + pizzaria };
        //                    return Json(resultado);
        //                }

        //            }
        //            else
        //            {
        //                this.Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro ao tentar criar");
        //                resultado = new Retorno() { msgRetorno = "Parâmetro recebido parece não estar certo, valor: " + id };
        //                return Json(resultado);
        //            }

        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        this.Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro ao tentar buscar os produtos");
        //        resultado = new Retorno() { msgRetorno = e.Message };
        //        return Json(resultado);
        //    }

        //}

        //[Route("pizzarias/{pizzariaId}/products/{productId}")]
        //[HttpDelete]
        //public JsonResult<Retorno> DeleteProductInPizzaria(int? pizzariaId, int? productId)
        //{
        //    Retorno resultado = new Retorno();
        //    Pizzaria pizzaria = new Pizzaria();

        //    try
        //    {
        //        using (var repositorio = new PizzariaRepositorio())
        //        {
        //            if (pizzariaId != null && productId != null)
        //            {

        //                pizzaria = repositorio.Find(pizzariaId);
        //                if (verificaProdutoEmPedidos(productId) != true)
        //                {
        //                    dynamic deletaProduto = DeletaProduto(productId);

        //                    if (deletaProduto.deletado)
        //                    {
        //                        resultado = new Retorno() { msgRetorno = "Operação Realizada com sucesso" };

        //                        this.Request.CreateResponse(HttpStatusCode.NoContent, "Produto excluído  com sucesso");

        //                        return Json(resultado);
        //                    }
        //                    else
        //                    {
        //                        resultado = new Retorno() { msgRetorno = deletaProduto.msg };

        //                        this.Request.CreateResponse(HttpStatusCode.BadRequest, "Erro ao deletar o produto");

        //                        return Json(resultado);
        //                    }
                            
                            
        //                }
        //                else
        //                {
        //                    this.Request.CreateResponse(HttpStatusCode.BadRequest, "Produto vinculado a pedido");
        //                    resultado = new Retorno() { msgRetorno = "Produto vinculado a pdido"};
        //                    return Json(resultado);
        //                }

        //            }
        //            else
        //            {
        //                this.Request.CreateResponse(HttpStatusCode.BadRequest, "Parâmetros incorretos");
        //                resultado = new Retorno() { msgRetorno = "Parâmetro recebido parece não estar certo, valor pizzariaId: " + pizzariaId + " valor productId: " + productId };
        //                return Json(resultado);
        //            }

        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        this.Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro ao tentar deletar");
        //        resultado = new Retorno() { msgRetorno = e.Message };
        //        return Json(resultado);
        //    }

        //}

        //[Route("pizzarias/{id}/orders/")]
        //[HttpGet]
        //public JsonResult<Retorno> ListarPedidos(int? id)
        //{
        //    Retorno resultado = new Retorno();

        //    try
        //    {
        //        using (var repositorio = new PizzariaRepositorio())
        //        {
        //            if (id != null)
        //            {
        //                var pizzaria = repositorio.Find(id);

        //                resultado = new Retorno() { msgRetorno = "Operação Realizada com sucesso", entidade =  pizzaria.pedidos.OrderBy(p => p.criadoQuando)};
        //                this.Request.CreateResponse(HttpStatusCode.OK, "Uma lista com todos os pedidos solicitados a partir dos parametros de filtragem");
        //                return Json(resultado);

        //            }
        //            else
        //            {
        //                this.Request.CreateResponse(HttpStatusCode.BadRequest, "Parâmetros incorretos");
        //                resultado = new Retorno() { msgRetorno = "Parâmetro recebido parece não estar certo, valor id: " + id};
        //                return Json(resultado);
        //            }

        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        this.Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro ao tentar buscar os pedidos");
        //        resultado = new Retorno() { msgRetorno = e.Message };
        //        return Json(resultado);
        //    }

        //}

        //[Route("pizzarias/{pizzariaId}/orders/{orderId}/deliver")]
        //[HttpPost]
        //public JsonResult<Retorno> MudaStatusDeliver(int? pizzariaId, int? orderId)
        //{
        //    return AtualizaStatus(pizzariaId, orderId, "SAIU_PARA_ENTREGA");
        //}

        //[Route("pizzarias/{pizzariaId}/orders/{orderId}/finish")]
        //[HttpPost]
        //public JsonResult<Retorno> MudaStatusFinish(int? pizzariaId, int? orderId)
        //{
        //    return AtualizaStatus(pizzariaId, orderId, "ENTREGE");
        //}

        //private bool verificaProdutoEmPedidos(int? produtoId)
        //{
        //    Produto produto = new Produto();
        //    Produto prod = new Produto();

        //    using (var repositorioProduto = new ProdutoRepositorio())
        //    {
        //        produto = repositorioProduto.Find(produtoId);
        //    }
        //    using (var repositorioPedido = new PedidoRepositorio())
        //    {
        //        var pedidos = repositorioPedido.GetAll().ToArray();

        //        foreach (var pedido in pedidos)
        //        {
        //            prod = pedido.produtos.Where(p => p.nome == produto.nome && p.Id == produto.Id).SingleOrDefault();
        //        }
        //    }
        //    if (prod != null)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //private dynamic DeletaProduto(int? productId)
        //{
        //    try
        //    {
        //        using (var repositorio = new ProdutoRepositorio())
        //        {
        //            repositorio.Excluir(p => p.Id == productId);
        //            repositorio.SalvarTodos();

        //            return new { deletado = true, msg = "" };
        //        }
        //    }
        //    catch(Exception e)
        //    {
        //        return new { deletado = false, msg = e.Message };
        //    }
            
                
        //}

        //private JsonResult<Retorno> AtualizaStatus(int? pizzariaId, int? orderId, string nomeStatus)
        //{
        //    Retorno resultado = new Retorno();

        //    try
        //    {
        //        using (var repositorio = new PedidoRepositorio())
        //        {
        //            if (orderId != null)
        //            {
        //                var pedido = repositorio.Find(orderId);
        //                pedido.status.statusPedido = nomeStatus;
        //                repositorio.Atualizar(pedido);

        //                resultado = new Retorno() { msgRetorno = "Operação Realizada com sucesso" };
        //                this.Request.CreateResponse(HttpStatusCode.NoContent, "Status do pedido alterado para saiu para entrega");
        //                return Json(resultado);

        //            }
        //            else
        //            {
        //                this.Request.CreateResponse(HttpStatusCode.BadRequest, "Parâmetros incorretos");
        //                resultado = new Retorno() { msgRetorno = "Parâmetro recebido parece não estar certo, valor orderId: " + orderId };
        //                return Json(resultado);
        //            }

        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        this.Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro ao tentar buscar os pedidos");
        //        resultado = new Retorno() { msgRetorno = e.Message };
        //        return Json(resultado);
        //    }
        //}

    }
}
