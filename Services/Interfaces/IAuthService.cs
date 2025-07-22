using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> UserExists(string username);
        Task<RegisterDto> Register(string username, string password, string role);
        Task<LoginDto> Login(string username, string password);
        Task<UserDto> GetUserByUsername(string username);
    }
}
