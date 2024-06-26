using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetsApp.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public int PetID { get; set; }
  
        public PetsModel Pet { get; set; }
        public int UserID { get; set; }
        public UserModels User { get; set; }

    }
}
