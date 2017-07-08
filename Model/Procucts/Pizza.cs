using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Procucts
{
    public class Pizza : Product
    {
        public List<ProductInstanceEntity> productInstance { get; set; }
    }
}
