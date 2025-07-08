using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repositories.Interfaces
{
    public interface ICountryRepository
    {
        Task<ICollection<Country>> GetCountries();
        Task<Country>? GetCountry(int countryId);
        Task<Country>? GetCountryOfAnOwner(int ownerId);
        Task<ICollection<Owner>> GetOwnersByCountry(int countryId);
        Task<bool> CountryExists(int countryId);
        Task<bool> CountryExists(string countryName);
        Task<bool> CreateCountry(Country country);
        Task<bool> UpdateCountry(Country country);
        Task<bool> DeleteCountry(Country country);
        Task<bool> Save();
    }
}
