using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum OrderStatus 
    {
        WAITING = 1,
        OUT_FOR_DELIVERY = 2,
        DONE = 3,
        CANCELED = 4
    }
}
