using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Models;
using AutoMapper;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;
        private readonly IMapper _mapper;

        public PokemonController(IPokemonService pokemonService, IMapper mapper)
        {
            _pokemonService = pokemonService;
            _mapper = mapper;
        }
        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(500)]
        public IActionResult GetPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(_pokemonService.GetPokemons());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemons);
        }
        [Authorize(Roles = "User,Admin")]
        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokeId)
        {
            if (!_pokemonService.PokemonExists(pokeId)) return NotFound();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var pokemon = _mapper.Map<PokemonDto>(_pokemonService.GetPokemon(pokeId));
            return Ok(pokemon);
        }
        [Authorize(Roles = "User,Admin")]
        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int pokeId)
        {
            if (!_pokemonService.PokemonExists(pokeId)) return NotFound();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var rating = _pokemonService.GetPokemonRating(pokeId);
            return Ok(rating);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int categoryId, [FromBody] PokemonDto createPokemon)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (createPokemon == null) return BadRequest(ModelState);
            var pokemon = _pokemonService.GetPokemons()
                                        .Where(o => o.Name.Trim().ToUpper() == createPokemon.Name.Trim().ToUpper())
                                        .FirstOrDefault();
            if (pokemon != null)
            {
                ModelState.AddModelError("", "Pokemon already exists");
                return StatusCode(422, ModelState);
            }
            var pokemonMap = _mapper.Map<Pokemon>(createPokemon);
            if (!_pokemonService.CreatePokemon(ownerId, categoryId, pokemonMap))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the pokemon {createPokemon}");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{pokeId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePokemon(int pokeId, [FromBody] PokemonDto updatePokemon)
        {
            if (updatePokemon == null) return BadRequest(ModelState);
            if (pokeId != updatePokemon.Id) return BadRequest(ModelState);
            if (!_pokemonService.PokemonExists(pokeId)) return NotFound();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var pokemonMap = _mapper.Map<Pokemon>(updatePokemon);
            if (!_pokemonService.UpdatePokemon(pokemonMap))
            {
                ModelState.AddModelError("", $"Something went wrong updating the pokemon {updatePokemon.Name}");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully updated");
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{pokeId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeletePokemon(int pokeId)
        {
            if (!_pokemonService.PokemonExists(pokeId)) return NotFound();
            var pokemonToDelete = _pokemonService.GetPokemon(pokeId);
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!_pokemonService.DeletePokemon(pokemonToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting the pokemon {pokemonToDelete.Name}");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully deleted");
        }
    }
}