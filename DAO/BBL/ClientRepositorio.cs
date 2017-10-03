using DAO.Repositorio;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.BBL
{
    public class ClientRepositorio : Repositorio<Client>
    {
        public dynamic AdicionarBBL(Client client)
        {
            try
            {
                using (var repositorio = this)
                {
                    repositorio.Adicionar(client);
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
    }
}
