using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.MapperManual
{
    public static class MapperManual
    {
        public static Pizzaria MontaModel(Pizzaria p)
        {
            Pizzaria pizzaria = new Pizzaria();
            pizzaria.name = p.name;
            pizzaria.PizzariaId = p.PizzariaId;
            pizzaria.deliveryTax = p.deliveryTax;

            if (p.menus != null)
            {
                pizzaria.menus = p.menus;
            }

            if (p.paymentMethods != null)
            {
                pizzaria.paymentMethods = p.paymentMethods;
            }

            if (p.orders != null)
            {
                pizzaria.orders = p.orders;
            }

            return pizzaria;
        }
    }
}
