using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetsApp.Models
{
    public enum OrderStatus
    {
        Ordered,
        OrderConfirmed,
        Delivering,
        SuccessfulDelivery,
        Cancelled
    }

}
