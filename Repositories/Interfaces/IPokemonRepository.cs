using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repositories.Interfaces
{
    public interface IPokemonRepository
    {
        Task<ICollection<Pokemon>> GetPokemons();
        Task<Pokemon?> GetPokemon(int id);
        Task<Pokemon?> GetPokemon(string name);
        Task<decimal> GetPokemonRating(int pokeid);
        Task<bool> PokemonExists(int id);
        Task<bool> CreatePokemon(int ownerId, int categoryId, Pokemon pokemon);
        Task<bool> UpdatePokemon(Pokemon pokemon);
        Task<bool> DeletePokemon(Pokemon pokemon);
        Task<bool> Save();
    }
}
