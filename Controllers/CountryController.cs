using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;
using PokemonReviewApp.Services.Interfaces;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;
        private readonly IMapper _mapper;
        public CountryController(ICountryService countryService, IMapper mapper)
        {
            _countryService = countryService;
            _mapper = mapper;
        }
        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CountryDto>))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _countryService.GetCountries();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(countries);
        }
        [Authorize(Roles = "User,Admin")]
        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCountry(int countryId)
        {
            if (!await _countryService.CountryExists(countryId)) return NotFound();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var country =await _countryService.GetCountry(countryId);
            return Ok(country);
        }
        [Authorize(Roles = "User,Admin")]
        [HttpGet("owners/{countryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<OwnerDto>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetOwnersByCountry(int countryId)
        {
            if (!await _countryService.CountryExists(countryId)) return NotFound();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var owners = await _countryService.GetOwnersByCountry(countryId);
            return Ok(owners);
        }
        [Authorize(Roles = "User,Admin")]
        [HttpGet("owner/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCountryOfAnOwner(int ownerId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var country = await _countryService.GetCountryOfAnOwner(ownerId);
            return Ok(country);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CountryDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCountry([FromBody] CountryDto createCountry)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (createCountry == null) return BadRequest(ModelState);
            if (!await _countryService.CreateCountry(createCountry))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the country {createCountry.Name}");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{countryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCountry(int countryId, [FromBody] CountryDto updateCountry)
        {
            if (updateCountry == null) return BadRequest(ModelState);
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!await _countryService.UpdateCountry(updateCountry))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the country {updateCountry.Name}");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully updated");
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{countryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCountry(int countryId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var countryToDelete = await _countryService.GetCountry(countryId);
            if (!await _countryService.DeleteCountry(countryToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the country {countryToDelete.Name}");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully deleted");
        }
    }
}
