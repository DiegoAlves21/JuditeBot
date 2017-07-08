using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Procucts
{
    public class ProductInstance <T> : ICommodity
    {
        public int Id { get; set; }

        public double cost { get; set; }

        public int productSizeId { get; set; }

        public ProductSize productSize { get; set; }

        public double getPrice()
        {
            return this.cost;
        }
    }
}
