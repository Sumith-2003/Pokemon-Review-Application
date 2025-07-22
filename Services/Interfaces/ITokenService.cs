using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(UserDto user);
    }
}
