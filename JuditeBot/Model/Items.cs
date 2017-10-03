using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JuditeBot.Model
{
    public class Items
    {
        public List<Products> products { get; set; }
        public List<Products[]> mixedPizzas { get; set; }
    }
}