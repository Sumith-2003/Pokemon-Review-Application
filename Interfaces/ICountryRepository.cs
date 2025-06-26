using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries();
        Country? GetCountry(int countryId);
        ICollection<Owner> GetOwnersByCountry(int countryId);
        bool CountryExists(int countryId);
        bool CountryExists(string countryName);
    }
}
