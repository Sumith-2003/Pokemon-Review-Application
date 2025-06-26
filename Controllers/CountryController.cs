using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        [ProducesResponseType(500)]
        public IActionResult GetCountries()
        {
            var countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(countries);
        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountry(int countryId)
        {
            if (!_countryRepository.CountryExists(countryId)) return NotFound();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountry(countryId));
            return Ok(country);
        }

        [HttpGet("owners/{countryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        public IActionResult GetOwnersByCountry(int countryId)
        {
            if (!_countryRepository.CountryExists(countryId)) return NotFound();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var owners = _mapper.Map<List<Owner>>(_countryRepository.GetOwnersByCountry(countryId));
            return Ok(owners);
        }
    }
}
