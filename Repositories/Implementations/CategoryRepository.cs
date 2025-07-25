﻿using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Helpers;
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
        public async Task<bool> CategoryExists(int categoryId) =>
            await _context.Categories.AnyAsync(c => c.Id == categoryId);

        public async Task<bool> CreateCategory(Category category)
        {
            //Change Tracker is EF Core's internal system to monitor entity states(Added, Modified, Deleted). 
            await _context.Categories.AddAsync(category);
            //Save changes to the database
            return await Save();
        }
        public async Task<ICollection<Category>> GetCategories() =>
            await _context.Categories.OrderBy(c => c.Id).ToListAsync();
        public async Task<ICollection<Category>> GetCategoriesAsync(PaginationParams query)
        {
            var categories = _context.Categories.AsQueryable();

            // 🔍 Filtering
            if (!string.IsNullOrEmpty(query.Search))
            {
                categories = categories.Where(c => c.Name.Contains(query.Search));
            }

            // 🔃 Sorting
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                switch (query.SortBy.ToLower())
                {
                    case "name":
                        categories = categories.OrderBy(c => c.Name);
                        break;
                    case "id":
                    default:
                        categories = categories.OrderBy(c => c.Id);
                        break;
                }
            }
            else
            {
                categories = categories.OrderBy(c => c.Id); // default sort
            }

            // 📄 Pagination
            categories = categories
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize);

            return await categories.ToListAsync();
        }

        public async Task<Category> GetCategory(int categoryId) =>
            await _context.Categories
                          .Where(c => c.Id == categoryId)
                          .FirstAsync();

        public async Task<ICollection<Pokemon>> GetPokemonsByCategory(int categoryId) =>
            await _context.Pokemon
                          .Where(p => p.PokemonCategories.Any(pc => pc.Category.Id == categoryId))
                          .OrderBy(p => p.Name)
                          .ToListAsync();

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