using Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Pizzaria 
    {
        public int Id { get; set; }
        public string nome { get; set; }
        public double taxaEntrega { get; set; }

        public virtual IList<MeioPagamento> meioPagamento { get; set; }

        public virtual IList<Produto> produtos { get; set; }

        public virtual IList<Pedido> pedidos { get; set; }

        public virtual IList<Usuario> usuarios { get; set; }

    }
}
