using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JuditeBot.Model
{
    public class OrdersModelView
    {
        public string clientName { get; set; }
        public string address { get; set; }
        public string status { get; set; }
        public string paymentMethod { get; set; }
        public string createdAt { get; set; }
        public Items items { get; set; }
    }
}