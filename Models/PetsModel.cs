using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetsApp.Models
{
    public class PetsModel
    {
        public int ID { get; set; }
        public string NamePets { get; set; }

        public int PetsTypeID { get; set; }
        public TypePets? PetTypes { get; set; }
        public string Gender { get; set; }
        public double Size { get; set; }
        public int Age { get; set; }
        public string Description { get; set; }
        public string Species { get; set; }
        public string Image { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public int Unit { get; set; }




    }
}
