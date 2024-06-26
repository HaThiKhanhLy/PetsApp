using Newtonsoft.Json;
using PetsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetsApp.Services
{
    public class TypePetsServices
    {
        private readonly static IEnumerable<TypePets> typePets = new List<TypePets>
        {
            new TypePets
            {
                ID = 1,
                Name = "Dog",
                Image= "dog1.png"
            },
            new TypePets
            {
                 ID = 2,
                Name = "Cat",
                Image= "cat.png"
            },
            new TypePets
            {
                 ID = 3,
                Name = "Rabbit",
                Image= "rabbit.png"
            },
            new TypePets
            {
                 ID = 4,
                Name = "Fish",
                Image= "fish.png"
            },
            new TypePets
            {
                ID = 5,
                Name = "Bird",
                Image= "parrot.png"
            },
            new TypePets
            {
                 ID = 6,
                Name = "Cat",
                Image= "cat.png"
            },
            new TypePets
            {
                 ID = 7,
                Name = "Rabbit",
                Image= "rabbit.png"
            },
            new TypePets
            {
                 ID = 8,
                Name = "Orther",
                Image= "footprint.png"
            },
        }; 
        public IEnumerable<TypePets> GetAllTypePets() => typePets;
    }
    

}
