using Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Pizzaria : IEntity
    {
        public int Id { get; set; }
        public string nome { get; set; }
        public double taxaEntrega { get; set; }
        public IList<MeioPagamento.meioPagamento> meioPagamento { get; set; }

        public int produtoId { get; set; }
        public virtual IList<Produto> cardapio { get; set; }

        public int pedidoId { get; set; }
        public virtual IList<Pedido> pedidos { get; set; }

        public int usuarioId { get; set; }
        public virtual Usuario usuario { get; set; }
    }
}
