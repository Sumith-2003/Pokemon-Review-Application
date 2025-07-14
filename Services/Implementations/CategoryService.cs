using AutoMapper;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Helpers;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repositories.Interfaces;
using PokemonReviewApp.Services.Interfaces;

namespace PokemonReviewApp.Services.Repository
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepository categoryRepository,IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public Task<bool> CategoryExists(int categoryId)
        {
            return _categoryRepository.CategoryExists(categoryId);
        }

        public async Task<bool> CreateCategory(CategoryDto categoryDto)
        {
            if (await _categoryRepository.CategoryExists(categoryDto.Id))
                throw new ArgumentException("Category already exists");
            var category = _mapper.Map<Category>(categoryDto);
            return await _categoryRepository.CreateCategory(category);
        }

        public async Task<bool> DeleteCategory(int categoryId)
        {
            if (! await _categoryRepository.CategoryExists(categoryId))
                throw new ArgumentException("Category does not exists");
            return await _categoryRepository.DeleteCategory(categoryId);
        }

        public async Task<ICollection<CategoryDto>> GetCategories()
        {
            var getCategories = await _categoryRepository.GetCategories();
            var categories = _mapper.Map<ICollection<CategoryDto>>(getCategories);
            return categories;
        }
        public async Task<ICollection<CategoryDto>> GetCategories(PaginationParams query)
        {
            var categories = await _categoryRepository.GetCategoriesAsync(query);
            return _mapper.Map<ICollection<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> GetCategory(int categoryId)
        {
            if (!await _categoryRepository.CategoryExists(categoryId))
                throw new ArgumentException("Category does not exists");
            var getCategory = await _categoryRepository.GetCategory(categoryId);
            var category = _mapper.Map<CategoryDto>(getCategory);
            return category;
        }

        public async Task<ICollection<PokemonDto>> GetPokemonsByCategory(int categoryId)
        {
            if (!await _categoryRepository.CategoryExists(categoryId))
                throw new ArgumentException("Category does not exists");
            var getPokemons = await _categoryRepository.GetPokemonsByCategory(categoryId);
            var pokemons = _mapper.Map<ICollection<PokemonDto>>(getPokemons);
            return pokemons;
        }

        public async Task<bool> Save()
        {
            var saved = await _categoryRepository.Save();
            return saved;
        }

        public async Task<bool> UpdateCategory(CategoryDto categoryDto)
        {
            if (!await _categoryRepository.CategoryExists(categoryDto.Id))
                throw new ArgumentException("Category does not exists");
            var category = _mapper.Map<Category>(categoryDto);
            return await _categoryRepository.UpdateCategory(category);
        }
    }
}