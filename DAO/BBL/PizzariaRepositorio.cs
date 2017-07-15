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
        public void AdicionarBBL(Pizzaria pizzaria)
        {
            using (var repositorio = this)
            {
                repositorio.Adicionar(pizzaria);
                repositorio.SalvarTodos();
                repositorio.Dispose();
            }
                
        }

        public IQueryable<Pizzaria> GetBBL(Func<Pizzaria, bool> predicate)
        {
            using (var repositorio = this)
            {
                var pizzaria = repositorio.Get(predicate);
                return pizzaria;
            }

        }
    }
}
