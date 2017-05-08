using Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Produto
    {
        public int Id { get; set; }
        public string nome { get; set; }
        public double valor { get; set; }

        public virtual List<Pedido> pedidos { get; set; }

        public virtual List<Pizzaria> pizzarias { get; set; }

        public virtual PizzaTamanho pizzasTamanhos { get; set; }
    }
}
