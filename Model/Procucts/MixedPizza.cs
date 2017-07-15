using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Procucts
{
    public class MixedPizza : ICommodity
    {
        public int Id { get; set; }

        public int orderId { get; set; }
        public virtual Order order { get; set; }

        //public int productInstanceId { get; set; }
        public virtual List<ProductInstance> productInstances { get; set; }

        public double getPrice()
        {
            //Implementar
            throw new NotImplementedException();
        }
    }
}
