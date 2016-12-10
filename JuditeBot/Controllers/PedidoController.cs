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
    [RoutePrefix("JuditeBot/Pedido")]
    public class PedidoController : ApiController
    {
        [Route("Adicionar")]
        [HttpPost]
        public JsonResult<Retorno> Adicionar([FromBody]Pedido pedido)
        {
            Retorno resultado = new Retorno();
            Produto produto = new Produto();
            Pizzaria pizzaria = new Pizzaria();
            StatusPedido status = new StatusPedido();
            status.statusPedido = "Aguardando pagamento";
            try
            {
                using (var pizzariaRepositorio = new PizzariaRepositorio())
                {
                    var p = pizzariaRepositorio.Find(1);
                    pizzaria = MapperManual.MontaModel(p);
                }
                using (var produtoRepositorio = new ProdutoRepositorio())
                {
                    produto = produtoRepositorio.Find(2);
                }
                using (var repositorio = new PedidoRepositorio())
                {
                    if (pedido == null)
                    {
                        resultado = new Retorno() { msgRetorno = "Parâmetro recebido parece não estar certo, valor: " + pedido };
                    }
                    pedido.produtos = new List<Produto>();
                    pedido.meioPagamento = new MeioPagamento();
                    pedido.meioPagamento = pizzaria.meioPagamento[0];
                    pedido.status = status;
                    pedido.produtos.Add(produto);
                    repositorio.Adicionar(pedido);
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
    }
}
