using DAO.Repositorio;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.BBL
{
    public class PedidoRepositorio : Repositorio<Order>
    {
        public dynamic AdicionarBBL(Order order)
        {
            try
            {
                using (var repositorio = this)
                {
                    repositorio.Adicionar(order);
                    repositorio.SalvarTodos();
                    repositorio.Dispose();
                    return new { temErro = false, msgRetorno = "Operação Realizada com sucesso" };
                }
            }
            catch (Exception e)
            {
                return new { temErro = true, msgRetorno = e.Message };
            }

        }

        public void AtualizarBBL(Order order)
        {
            using (var repositorio = this)
            {
                repositorio.Atualizar(order);
                repositorio.SalvarTodos();
                repositorio.Dispose();
            }

        }
    }
}
