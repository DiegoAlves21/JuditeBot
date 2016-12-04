using DAO.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Models
{
    public class Pedido : IEntity
    {
        public int Id { get; set; }
        public string nomeCliente { get; set; }
        public string endereco { get; set; }

        public string statusId { get; set; }
        public virtual StatusPedido.statusPedido status { get; set; }

        public int meioPagamentoId { get; set; }
        public virtual MeioPagamento.meioPagamento meioPagamento { get; set; }

        public int produtoId { get; set; }
        public Dictionary<Produto, Double> produtos { get; set; }

    }
}
