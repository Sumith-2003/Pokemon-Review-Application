using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> UserExists(string username);
        Task<User> Register(string username, string password);
        Task<User> Login(string username, string password);
    }
}
