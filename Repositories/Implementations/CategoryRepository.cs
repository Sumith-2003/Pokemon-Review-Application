using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repositories.Interfaces;

namespace PokemonReviewApp.Repositories.Implementations
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;
        public CategoryRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> CategoryExists(int categoryId)
        {
            return await _context.Categories.AnyAsync(c => c.Id == categoryId);
        }

        public async Task<bool> CreateCategory(Category category)
        {
            //Change Tracker is EF Core's internal system to monitor entity states(Added, Modified, Deleted). 
            await _context.Categories.AddAsync(category);
            //Save changes to the database
            return await Save();
        }
        public async Task<ICollection<Category>> GetCategories()
        {
            return await _context.Categories.OrderBy(c => c.Id).ToListAsync();
        }

        public async Task<Category> GetCategory(int categoryId)
        {
            return await _context.Categories
                        .Where(c => c.Id == categoryId)
                        .FirstAsync();
        }

        public async Task<ICollection<Pokemon>> GetPokemonsByCategory(int categoryId)
        {
            return await _context.Pokemon
                .Where(p => p.PokemonCategories.Any(pc => pc.Category.Id == categoryId))
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<bool> Save()
        {
            try
            {
                var saved = await _context.SaveChangesAsync();
                return saved > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> UpdateCategory(Category category)
        {
            _context.Update(category);
            return await Save();
        }
        public async Task<bool> DeleteCategory(int categoryId)
        {
            var category =await _context.Categories.FindAsync(categoryId);
            if (category == null)
            {
                return false;
            }
            _context.Categories.Remove(category);
            return await Save();
        }
    }
}