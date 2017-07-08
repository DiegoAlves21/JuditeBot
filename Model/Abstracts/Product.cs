using Model.Interfaces;
using Model.Procucts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public abstract class Product
    {
        public int Id { get; set; }
        public string name { get; set; }
        public bool avaible { get; set; }

        //public List<ProductInstance<>> productInstance { get; set; }

        //public double valor { get; set; }

        //public virtual List<Pedido> pedidos { get; set; }

        //public virtual List<Pizzaria> pizzarias { get; set; }

    }
}
