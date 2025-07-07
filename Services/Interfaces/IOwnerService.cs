using PokemonReviewApp.Models;

namespace PokemonReviewApp.Services.Interfaces
{
    public interface IOwnerService
    {
        ICollection<Owner> GetOwners();
        Owner? GetOwner(int ownerId);
        ICollection<Owner> GetOwnerOfAPokemon(int pokeId);
        ICollection<Pokemon> GetPokemonsByOwner(int ownerId);
        bool OwnerExists(int ownerId);
        bool CreateOwner(Owner owner);
        bool UpdateOwner(Owner owner);
        bool DeleteOwner(Owner owner);
        bool Save();
    }
}
