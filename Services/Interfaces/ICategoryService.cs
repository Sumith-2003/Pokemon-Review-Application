using PokemonReviewApp.Models;

namespace PokemonReviewApp.Services.Interfaces
{
    public interface ICategoryService
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int categoryId);
        ICollection<Pokemon> GetPokemonsByCategory(int categoryId);
        bool CategoryExists(int categoryId);
        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(int categoryId);
        bool Save();
    }
}
