using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;
        public CategoryRepository(DataContext context)
        {
            _context = context;
        }
        public bool CategoryExists(int categoryId)
        {
            return _context.Categories.Any(c => c.Id == categoryId);
        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories.OrderBy(c => c.Id).ToList();
        }

        public Category GetCategory(int categoryId)
        {
            return _context.Categories
                .Where(c => c.Id == categoryId)
                .First();
        }

        public ICollection<Pokemon> GetPokemonsByCategory(int categoryId)
        {
            return _context.Pokemon
                .Where(p => p.PokemonCategories.Any(pc => pc.Category.Id == categoryId))
                .OrderBy(p => p.Name)
                .ToList();
        }
    }
}
