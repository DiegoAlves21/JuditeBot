using Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Procucts
{
    public class MixedPizza : ICommodity
    {
        public int Id { get; set; }
        public List<Pizza> flavors { get; set; }
        public PizzaSize size { get; set; }

        public double getPrice()
        {
            //Implementar
            throw new NotImplementedException();
        }
    }
}
