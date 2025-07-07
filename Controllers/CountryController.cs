using AutoMapper;
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
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        [ProducesResponseType(500)]
        public IActionResult GetCountries()
        {
            var countries = _mapper.Map<List<CountryDto>>(_countryService.GetCountries());
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(countries);
        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountry(int countryId)
        {
            if (!_countryService.CountryExists(countryId)) return NotFound();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var country = _mapper.Map<CountryDto>(_countryService.GetCountry(countryId));
            return Ok(country);
        }

        [HttpGet("owners/{countryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        public IActionResult GetOwnersByCountry(int countryId)
        {
            if (!_countryService.CountryExists(countryId)) return NotFound();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var owners = _mapper.Map<List<OwnerDto>>(_countryService.GetOwnersByCountry(countryId));
            return Ok(owners);
        }
        [HttpGet("owner/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountryOfAnOwner(int ownerId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var country = _mapper.Map<CountryDto>(_countryService.GetCountryOfAnOwner(ownerId));
            return Ok(country);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry([FromBody] CountryDto createCountry)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (createCountry == null) return BadRequest(ModelState);
            var country = _countryService.GetCountries()
                                           .Where(c => c.Name.Trim().ToUpper() == createCountry.Name.TrimEnd().ToUpper())
                                           .FirstOrDefault();
            if (country != null)
            {
                ModelState.AddModelError("", "Country already exists");
                return StatusCode(422, ModelState);
            }
            var countryMap = _mapper.Map<Country>(createCountry);
            if (!_countryService.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the country {createCountry.Name}");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }
        [HttpPut("{countryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCountry(int countryId, [FromBody] CountryDto updateCountry)
        {
            if (updateCountry == null) return BadRequest(ModelState);
            if (countryId != updateCountry.Id) return BadRequest(ModelState);
            if (!_countryService.CountryExists(countryId)) return NotFound();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var countryMap = _mapper.Map<Country>(updateCountry);
            if (!_countryService.UpdateCountry(countryMap))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the country {updateCountry.Name}");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully updated");
        }
        [HttpDelete("{countryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCountry(int countryId)
        {
            if (!_countryService.CountryExists(countryId)) return NotFound();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var countryToDelete = _countryService.GetCountry(countryId);
            if (!_countryService.DeleteCountry(countryToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the country {countryToDelete.Name}");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully deleted");
        }
    }
}
