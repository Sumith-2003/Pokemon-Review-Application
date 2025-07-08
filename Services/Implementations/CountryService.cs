using AutoMapper;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repositories.Interfaces;
using PokemonReviewApp.Services.Interfaces;

namespace PokemonReviewApp.Services.Repository
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        public CountryService(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }
        public async Task<bool> CountryExists(int countryId)
        {
            return await _countryRepository.CountryExists(countryId);
        }

        public async Task<bool> CountryExists(string countryName)
        {
            return await _countryRepository.CountryExists(countryName);
        }

        public async Task<bool> CreateCountry(CountryDto countryDto)
        {
            if (await _countryRepository.CountryExists(countryDto.Name))
                throw new ArgumentException("Country already exists");
            var country = _mapper.Map<Country>(countryDto);
            return await _countryRepository.CreateCountry(country);
        }

        public async Task<bool> DeleteCountry(CountryDto countryDto)
        {
            if (!await _countryRepository.CountryExists(countryDto.Name))
                throw new ArgumentException("Country does not exists");
            var deleteCountry = _mapper.Map<Country>(countryDto);
            return await _countryRepository.DeleteCountry(deleteCountry);
        }

        public async Task<ICollection<CountryDto>> GetCountries()
        {
            var getCountries = await _countryRepository.GetCountries();
            var countries = _mapper.Map<ICollection<CountryDto>>(getCountries);
            return countries;
        }

        public async Task<CountryDto>? GetCountry(int countryId)
        {
            if (!await _countryRepository.CountryExists(countryId))
                throw new ArgumentException("Country does not exists");
            var getCountry = await _countryRepository.GetCountry(countryId);
            var country = _mapper.Map<CountryDto>(getCountry);
            return country;
        }

        public async Task<CountryDto>? GetCountryOfAnOwner(int ownerId)
        {
            var getCountry = await _countryRepository.GetCountryOfAnOwner(ownerId);
            var country = _mapper.Map<CountryDto>(getCountry);
            if (country == null)
                throw new ArgumentException("Owner does not have a country");
            return country;
        }

        public async Task<ICollection<OwnerDto>> GetOwnersByCountry(int countryId)
        {
            if (!await _countryRepository.CountryExists(countryId))
                throw new ArgumentException("Country does not exists");
            var getOwners = await _countryRepository.GetOwnersByCountry(countryId);
            var owners = _mapper.Map<ICollection<OwnerDto>>(getOwners);
            if(owners == null) throw new ArgumentException("No owners found for this country");
            return owners;
        }

        public async Task<bool> Save()
        {
            var saved = await _countryRepository.Save();
            return saved;
        }

        public async Task<bool> UpdateCountry(CountryDto countryDto)
        {
            if (!await _countryRepository.CountryExists(countryDto.Id))
                throw new ArgumentException("Country does not exists");
            var updateCountry = _mapper.Map<Country>(countryDto);
            return await _countryRepository.UpdateCountry(updateCountry);
        }
    }
}
