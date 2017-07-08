using Model.Procucts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Beverage : Product
    {
        public List<ProductInstanceEntity> productInstance { get; set; }
    }
}
