using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Services.Interfaces
{
    public interface ICountryService
    {
        Task<ICollection<CountryDto>> GetCountries();
        Task<CountryDto>? GetCountry(int countryId);
        Task<CountryDto>? GetCountryOfAnOwner(int ownerId);
        Task<ICollection<OwnerDto>> GetOwnersByCountry(int countryId);
        Task<bool> CountryExists(int countryId);
        Task<bool> CountryExists(string countryName);
        Task<bool> CreateCountry(CountryDto countryDto);
        Task<bool> UpdateCountry(CountryDto countryDto);
        Task<bool> DeleteCountry(CountryDto countryDto);
        Task<bool> Save();
    }
}
