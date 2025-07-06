using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries();
        Country? GetCountry(int countryId);
        Country? GetCountryOfAnOwner(int ownerId);
        ICollection<Owner> GetOwnersByCountry(int countryId);
        bool CountryExists(int countryId);
        bool CountryExists(string countryName);
        bool CreateCountry(Country country);
        bool UpdateCountry(Country country);
        bool Save();
    }
}
