using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.MapperManual
{
    public static class MapperManual
    {
        public static Pizzaria MontaModel(Pizzaria p)
        {
            Pizzaria pizzaria = new Pizzaria();
            pizzaria.nome = p.nome;
            pizzaria.Id = p.Id;
            pizzaria.taxaEntrega = p.taxaEntrega;

            if (p.produtos != null)
            {
                pizzaria.produtos = p.produtos;
            }

            if (p.meioPagamento != null)
            {
                pizzaria.meioPagamento = p.meioPagamento;
            }

            if (p.pedidos != null)
            {
                pizzaria.pedidos = p.pedidos;
            }

            return pizzaria;
        }
    }
}
