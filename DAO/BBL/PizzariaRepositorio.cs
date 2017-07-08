using DAO.Repositorio;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.BBL
{
    public class PizzariaRepositorio : Repositorio<Pizzaria>
    {
        public dynamic AdicionarBBL(Pizzaria pizzaria)
        {
            try
            {
                using (var repositorio = this)
                {
                    repositorio.Adicionar(pizzaria);
                    repositorio.SalvarTodos();
                    repositorio.Dispose();
                    return new { temErro = false, msgRetorno = "Operação Realizada com sucesso" };
                }
            }
            catch(Exception e)
            {
                return new { temErro = true, msgRetorno = e.Message };
            }
                
        }
    }
}
