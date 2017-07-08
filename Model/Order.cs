using Model;
using Model.Interfaces;
using Model.Procucts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Order
    {
        public int Id { get; set; }
        public string clientName { get; set; }
        public string Address { get; set; }
        public DateTime created { get; set; }

        public int statusId { get; set; }
        public virtual OrderStatus status { get; set; }

        public int meioPagamentoId { get; set; }
        public virtual PaymentMethod meioPagamento { get; set; }

        public int itensId { get; set; }
        public virtual List<ICommodity> itens { get; set; }
    }
}
