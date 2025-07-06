using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext _context;
        public CountryRepository(DataContext context)
        {
            _context = context;
        }
        public bool CountryExists(int countryId)
        {
            return _context.Countries.Any(c => c.Id == countryId);
        }

        public bool CountryExists(string countryName)
        {
            return _context.Countries.Any(c => c.Name.Trim().ToUpper() == countryName.Trim().ToUpper());
        }

        public bool CreateCountry(Country country)
        {
            _context.Countries.Add(country);
            return Save();
        }

        public ICollection<Country> GetCountries()
        {
            return _context.Countries.OrderBy(c => c.Name).ToList();
        }

        public Country? GetCountry(int countryId)
        {
            return _context.Countries.FirstOrDefault(c => c.Id == countryId);
        }

        public Country? GetCountryOfAnOwner(int ownerId)
        {
            return _context.Countries
                .Where(c => c.Owners.Any(o => o.Id == ownerId))
                .FirstOrDefault();
        }

        public ICollection<Owner> GetOwnersByCountry(int countryId)
        {
            return _context.Owners
                .Where(o => o.Country.Id == countryId)
                .OrderBy(o => o.FirstName)
                .ToList();
        }
        public bool Save()
        {
            try
            {
                var saved = _context.SaveChanges();
                return saved > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool UpdateCountry(Country country)
        {
            _context.Update(country);
            return Save();
        }
    }
}
