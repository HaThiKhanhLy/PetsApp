using PetsApp.Pages;
using PetsApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetsApp.Repository
{
    internal interface IPetReponsitory
    {
        Task<List<PetsModel>> GetAllPets();
        Task<PetsModel> GetPetById(int petId);
        Task<bool> DeletePet(int userId, int petId);
    }
}
