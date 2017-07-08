using Model.Procucts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ProductInstanceEntity
    {
        public int Id { get; set; }

        public double cost { get; set; }

        public string Type { get; set; }

        public int productSizeId { get; set; }

        public ProductSize productSize { get; set; }

        public double getPrice()
        {
            return this.cost;
        }
    }
}
