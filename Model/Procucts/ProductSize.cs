﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Procucts
{
    public class ProductSize
    {
        public int Id { get; set; }
        public string name { get; set; }

        //public int productInstanceId { get; set; }
        public virtual IList<ProductInstance> productInstance { get; set; }
    }
}
