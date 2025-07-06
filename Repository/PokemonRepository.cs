using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _context;
        public PokemonRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var pokemonOwnerEntity = _context.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
            var pokemonCategoryEntity = _context.Categories.Where(c => c.Id == categoryId).FirstOrDefault();
            var pokemonOwner = new PokemonOwner()
            {
                Owner = pokemonOwnerEntity,
                Pokemon = pokemon
            };
            _context.Add(pokemonOwner);
            var pokemonCategory = new PokemonCategory()
            {
                Category = pokemonCategoryEntity,
                Pokemon = pokemon
            };
            _context.Add(pokemonCategory);
            _context.Add(pokemon);
            return Save();
        }

        public Pokemon? GetPokemon(int id)
        {
            return _context.Pokemon.Where(p => p.Id == id).FirstOrDefault();
        }

        public Pokemon? GetPokemon(string name)
        {
            return _context.Pokemon
                .Where(p => p.Name.Trim().ToLower() == name.Trim().ToLower())
                .FirstOrDefault();
        }

        public decimal GetPokemonRating(int pokeid)
        {
            return (decimal) _context.Reviews
                .Where(r => r.Pokemon.Id == pokeid)
                .Average(r => r.Rating);
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _context.Pokemon.OrderBy(p => p.Name).ToList();
        }

        public bool PokemonExists(int id)
        {
            return _context.Pokemon.Any(p => p.Id == id);
        }

        public bool Save()
        {
            try
            {
                var saved = _context.SaveChanges();
                return saved > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdatePokemon(Pokemon pokemon)
        {
            _context.Update(pokemon);
            return Save();
        }
    }
}
