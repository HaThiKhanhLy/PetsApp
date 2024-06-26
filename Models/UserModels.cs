using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetsApp.Models
{
    public class UserModels
    {

        public int ID { get; set; }
        public string FullName { get; set; }
        [StringLength(10, ErrorMessage = "Contact number should not exceed 10 digits")]
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [DataType(DataType.MultilineText)]
        public string Address { get; set; }
        public DateTime? DayOfBirth { get; set; }
        public string Gender { get; set; }

        [Display(AutoGenerateField = false)]
        public string Role { get; set; }

        [Display(AutoGenerateField = false)]
        public string Status { get; set; }


    }
}

