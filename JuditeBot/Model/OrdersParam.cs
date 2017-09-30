using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JuditeBot.Model
{
    public class OrdersParam
    {
        public int p { get; set; }
        public int per_page { get; set; }
        public string filterPaymentMethod { get; set; }
        public string filterStatus { get; set; }
        public string filterClientName { get; set; }
        public string s { get; set; }
        public string sDir { get; set; }
    }
}