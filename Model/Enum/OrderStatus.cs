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
        PREPARING = 2,
        OUT_FOR_DELIVERY = 3,
        DONE = 4,
        CANCELED = 5
    }
}
