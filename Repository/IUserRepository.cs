using PetsApp.Models;
namespace PetsApp.Repository
{
    public interface IUserRepository
    {
        Task<UserModels> Login(string email, string password);
        Task<UserModels> Register(string email, string password);
    }
}
