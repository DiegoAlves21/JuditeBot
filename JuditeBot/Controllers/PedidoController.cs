using DAO.BBL;
using DAO.MapperManual;
using Model;
using Model.Interfaces;
using Model.Procucts;
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
        public JsonResult<dynamic> Adicionar([FromBody]Order pedido)
        {
            dynamic resultado;
            List<ICommodity> produtos = new List<ICommodity>();
            Pizzaria pizzaria = new Pizzaria();
            OrderStatus status = new OrderStatus();
            status.description = "Aguardando pagamento";
            try
            {
                //using (var pizzariaRepositorio = new PizzariaRepositorio())
                //{
                //    var p = pizzariaRepositorio.Find(1);
                //    pizzaria = MapperManual.MontaModel(p);
                //}
                //using (var produtoRepositorio = new ProdutoRepositorio())
                //{
                //    produto = produtoRepositorio.Find(2);
                //}
                //using (var repositorio = new PedidoRepositorio())
                //{
                //    if (pedido == null)
                //    {
                //        resultado = new Retorno() { msgRetorno = "Parâmetro recebido parece não estar certo, valor: " + pedido };
                //    }
                //    pedido.produtos = new List<Product>();
                //    pedido.meioPagamento = new PaymentMethod();
                //    pedido.meioPagamento = pizzaria.paymentMethods[0];
                //    pedido.status = status;
                //    pedido.produtos.Add(produto);
                //    repositorio.Adicionar(pedido);
                //    repositorio.SalvarTodos();
                //    resultado = new Retorno() { msgRetorno = "Operação Realizada com sucesso" };

                //    return Json(resultado);
                //}
                //
                return null;
            }
            catch (Exception e)
            {
                resultado = new { msgRetorno = e.Message };
                return Json(resultado);
            }

        }
    }
}
