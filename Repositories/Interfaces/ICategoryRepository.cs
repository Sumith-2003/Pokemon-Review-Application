using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<bool> CategoryExists(int categoryId);
        Task<bool> CreateCategory(Category category);
        Task<ICollection<Category>> GetCategories();
        Task<Category> GetCategory(int categoryId);
        Task<ICollection<Pokemon>> GetPokemonsByCategory(int categoryId);
        Task<bool> UpdateCategory(Category category);
        Task<bool> DeleteCategory(int categoryId);
        Task<bool> Save();
    }
}
