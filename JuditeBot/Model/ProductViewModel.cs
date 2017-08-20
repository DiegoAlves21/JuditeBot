using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JuditeBot.Model
{
    public class ProductViewModel
    {
        public string name { get; set; }
        public string type { get; set; }
        public IList<Instances> instances { get; set; }
    }
}