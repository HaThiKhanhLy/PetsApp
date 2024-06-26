using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetsApp.Models
{
    public class OrderDetailsModel
    {
        public int OrderID { get; set; }
        public OrderModel Order { get; set; }
        public int PetId { get; set; }
        public PetsModel Pet { get; set; }
        public int Quantity { get; set; }
        public string unit { get; set; }
        public string payment { get; set; }
        public string Address { get; set; }
        public string RecipientName { get; set; }
        public string RecipientPhone { get; set; }

    }
}
