using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetsApp.Repository
{
    internal interface ICartReponsitory
    {
        Task<CartItem> UpdateQuantity(int userId, int petId, int newQuantity);
    }
}
