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
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerService _ownerService;
        private readonly ICountryService _countryService;
        private readonly IMapper _mapper;

        public OwnerController(
            IOwnerService ownerService,
            ICountryService countryService,
            IMapper mapper)
        {
            _ownerService = ownerService;
            _countryService = countryService;
            _mapper = mapper;
        }
        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(500)]
        public IActionResult GetOwners()
        {
            var pokemons = _mapper.Map<List<OwnerDto>>(_ownerService.GetOwners());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemons);
        }
        [Authorize(Roles = "User,Admin")]
        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetOwner(int ownerId)
        {
            if (!_ownerService.OwnerExists(ownerId)) return NotFound();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var owner = _mapper.Map<OwnerDto>(_ownerService.GetOwner(ownerId));
            return Ok(owner);
        }
        [Authorize(Roles = "User,Admin")]
        [HttpGet("{ownerId}/pokemons")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonsByOwner(int ownerId)
        {
            if (!_ownerService.OwnerExists(ownerId)) return NotFound();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var pokemons = _mapper.Map<List<PokemonDto>>(_ownerService.GetPokemonsByOwner(ownerId));
            return Ok(pokemons);
        }
        [Authorize(Roles = "User,Admin")]
        [HttpGet("{pokeId}/owners")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        public IActionResult GetOwnerOfAPokemon(int pokeId)
        {
            if (!_ownerService.GetOwnerOfAPokemon(pokeId).Any()) return NotFound();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var owners = _mapper.Map<List<OwnerDto>>(_ownerService.GetOwnerOfAPokemon(pokeId));
            return Ok(owners);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult CreateOwner([FromQuery] int countryId, [FromBody] OwnerDto createOwner)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (createOwner == null) return BadRequest(ModelState);
            var owner = _ownerService.GetOwners()
                                        .Where(o => o.LastName.Trim().ToUpper() == createOwner.LastName.TrimEnd().ToUpper())
                                        .FirstOrDefault();
            if (owner != null)
            {
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }
            var ownerMap = _mapper.Map<Owner>(createOwner);
            ownerMap.Country = _mapper.Map<Country>(_countryService.GetCountry(countryId));  //can do here or inside the repository
            if (!_ownerService.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the owner {createOwner.FirstName} {createOwner.LastName}");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{ownerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateOwner(int ownerId, [FromBody] OwnerDto updateOwner)
        {
            if (updateOwner == null) return BadRequest(ModelState);
            if (ownerId != updateOwner.Id) return BadRequest(ModelState);
            if (!_ownerService.OwnerExists(ownerId)) return NotFound();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var ownerMap = _mapper.Map<Owner>(updateOwner);
            if (!_ownerService.UpdateOwner(ownerMap))
            {
                ModelState.AddModelError("", $"Something went wrong updating the owner {updateOwner.FirstName} {updateOwner.LastName}");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully updated");
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{ownerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteOwner(int ownerId)
        {
            if (!_ownerService.OwnerExists(ownerId)) return NotFound();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var ownerToDelete = _ownerService.GetOwner(ownerId);
            if (!_ownerService.DeleteOwner(ownerToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting the owner {ownerToDelete.FirstName} {ownerToDelete.LastName}");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully deleted");
        }
    }
}
