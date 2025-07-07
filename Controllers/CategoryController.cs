using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Services.Interfaces;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDto>))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetCategories();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(categories);
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(CategoryDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCategory(int categoryId)
        {
            if (!await _categoryService.CategoryExists(categoryId)) return NotFound();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var category = await _categoryService.GetCategory(categoryId);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDto>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPokemonsByCategory(int categoryId)
        {
            if (! await _categoryService.CategoryExists(categoryId)) return NotFound();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var pokemons = await _categoryService.GetPokemonsByCategory(categoryId);
            if (pokemons == null) return NotFound();
            return Ok(pokemons);
        }
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto categoryCreate)
        {
            if (categoryCreate == null) return BadRequest(ModelState);
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (! await _categoryService.CreateCategory(categoryCreate))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }
        [HttpPut("{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryDto categoryUpdate)
        {
            if (categoryUpdate == null) return BadRequest(ModelState);
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!await _categoryService.UpdateCategory(categoryUpdate))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully updated");
        }
        [HttpDelete("{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (! await _categoryService.DeleteCategory(categoryId))
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully deleted");
        }
    }
}