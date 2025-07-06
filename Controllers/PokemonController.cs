using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Models;
using PokemonReviewApp.Interfaces;
using AutoMapper;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;

        public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(500)]
        public IActionResult GetPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemons);
        }

        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId)) return NotFound();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(pokeId));
            return Ok(pokemon);
        }

        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId)) return NotFound();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var rating = _pokemonRepository.GetPokemonRating(pokeId);
            return Ok(rating);
        }
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int categoryId, [FromBody] PokemonDto createPokemon)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (createPokemon == null) return BadRequest(ModelState);
            var pokemon = _pokemonRepository.GetPokemons()
                                        .Where(o => o.Name.Trim().ToUpper() == createPokemon.Name.Trim().ToUpper())
                                        .FirstOrDefault();
            if (pokemon != null)
            {
                ModelState.AddModelError("", "Pokemon already exists");
                return StatusCode(422, ModelState);
            }
            var pokemonMap = _mapper.Map<Pokemon>(createPokemon);
            if (!_pokemonRepository.CreatePokemon(ownerId, categoryId, pokemonMap))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the pokemon {createPokemon}");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }
    }
}