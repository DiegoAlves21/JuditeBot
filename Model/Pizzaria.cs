using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Pizzaria 
    {
        public int PizzariaId { get; set; }
        public string name { get; set; }
        public double deliveryTax { get; set; }

        public int paymentMethodsId { get; set; }
        public virtual IList<PaymentMethod> paymentMethods { get; set; }

        public int menusId { get; set; }
        public virtual IList<Product> menus { get; set; }

        public int ordersId { get; set; }
        public virtual IList<Order> orders { get; set; }

        public int usersId { get; set; }
        public virtual IList<Users> users { get; set; }

    }
}
