using Model.Enum;
using Model.Interfaces;
using Model.Procucts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Product
    {
        public int Id { get; set; }
        public string name { get; set; }
        public bool avaible { get; set; }

        public ProductType productType { get; set; }

        //public int productInstanceId { get; set; }
        public IList<ProductInstance> productInstance { get; set; }

        public int pizzariaId { get; set; }
        public Pizzaria pizzaria { get; set; }

    }
}
