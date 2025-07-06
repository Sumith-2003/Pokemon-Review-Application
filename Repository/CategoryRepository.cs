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

        public bool CreateCategory(Category category)
        {
            //Change Tracker is EF Core's internal system to monitor entity states(Added, Modified, Deleted). 
            _context.Categories.Add(category);
            //Save changes to the database
            return Save();
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

        public bool UpdateCategory(Category category)
        {
            _context.Update(category);
            return Save();
        }
    }
}
