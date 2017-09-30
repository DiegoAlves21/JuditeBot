using Model;
using Model.Interfaces;
using Model.Procucts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Order
    {
        public int Id { get; set; }
        public string clientName { get; set; }
        public string clientInformations { get; set; }
        public string Address { get; set; }
        public DateTime created { get; set; }

        public OrderStatus ordersStatus { get; set; }

        public int paymentMethodId { get; set; }
        public CPaymentMethod paymentMethod { get; set; }

        public virtual IList<ProductInstance> productInstances { get; set; }

        public int pizzariaId { get; set; }
        public virtual Pizzaria pizzaria { get; set; }

        //public int mixedPizzaId { get; set; }
        public virtual IList<MixedPizza> mixedPizzas { get; set; }
    }
}
