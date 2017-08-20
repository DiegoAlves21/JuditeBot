using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JuditeBot.Model
{
    public class OrdersViewModel
    {
        public string clientName { get; set; }
        public string Address { get; set; }
        public string paymentMethod { get; set; }
        public int[] flavors { get; set; }
    }
}