using PokemonReviewApp.Data;
using PokemonReviewApp.Models;
using PokemonReviewApp.Services.Interfaces;

namespace PokemonReviewApp.Services.Repository
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _context;
        public CategoryService(DataContext context)
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
        public bool DeleteCategory(int categoryId)
        {
            var category = _context.Categories.Find(categoryId);
            if (category == null)
            {
                return false;
            }
            _context.Categories.Remove(category);
            return Save();
        }
    }
}
