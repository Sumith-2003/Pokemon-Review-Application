using PokemonReviewApp.Dto;
using PokemonReviewApp.Helpers;

namespace PokemonReviewApp.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<ICollection<CategoryDto>> GetCategories();
        Task<ICollection<CategoryDto>> GetCategories(PaginationParams query);
        Task<CategoryDto> GetCategory(int categoryId);
        Task<ICollection<PokemonDto>> GetPokemonsByCategory(int categoryId);
        Task<bool> CategoryExists(int categoryId);
        Task<bool> CreateCategory(CategoryDto categoryDto);
        Task<bool> UpdateCategory(CategoryDto categoryDto);
        Task<bool> DeleteCategory(int categoryId);
        Task<bool> Save();
    }
}
