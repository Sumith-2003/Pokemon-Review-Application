﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Helpers;
using PokemonReviewApp.Services.Interfaces;
using PokemonReviewApp.Services.Repository;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryService> _logger;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public CategoryController(ILogger<CategoryService> logger,ICategoryService categoryService, IMapper mapper)
        {
            _logger = logger;
            _categoryService = categoryService;
            _mapper = mapper;
        }
        [Authorize(Roles ="User,Admin")]
        [HttpGet("all")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDto>))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllCategories()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var categories = await _categoryService.GetCategories();
            return Ok(categories);
        }
        [HttpGet("paginated")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCategories([FromQuery] PaginationParams query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categories = await _categoryService.GetCategories(query);
            return Ok(categories);
        }
        [Authorize(Roles = "User,Admin")]
        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(CategoryDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCategory(int categoryId)
        {
            _logger.LogInformation("Fetching category with ID {CategoryId}", categoryId);
            if (!await _categoryService.CategoryExists(categoryId)) return NotFound();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var category = await _categoryService.GetCategory(categoryId);
            if (category == null) return NotFound();
            _logger.LogInformation("Successfully retrieved category {Name}", category.Name);
            return Ok(category);
        }
        [Authorize(Roles = "User,Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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