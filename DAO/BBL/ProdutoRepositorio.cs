using DAO.Repositorio;
using Model;
using Model.Procucts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.BBL
{
    public class ProdutoRepositorio : Repositorio<Product>
    {
        public void AdicionarBBL(Product product)
        {
            using (var repositorio = this)
            {
                repositorio.Adicionar(product);
                repositorio.SalvarTodos();
                repositorio.Dispose();
            }

        }
    }
}
