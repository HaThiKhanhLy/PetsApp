using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace PetsApp.Models
{
    public partial class TypePets : ObservableObject
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
    }
}
