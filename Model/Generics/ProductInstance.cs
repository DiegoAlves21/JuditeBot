using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Procucts
{
    public class ProductInstance : ICommodity
    {
        public int Id { get; set; }

        public double cost { get; set; }

        //public int ordersId { get; set; }
        public virtual List<Order> orders { get; set; }

        public int productId { get; set; }
        public virtual Product product { get; set; }

        public int productSizeId { get; set; }
        public virtual ProductSize productSize { get; set; }

        public double getPrice()
        {
            return this.cost;
        }
    }
}
