using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repositories.Interfaces;

namespace PokemonReviewApp.Repositories.Implementations
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext _context;
        public CountryRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> CountryExists(int countryId)
        {
            return await _context.Countries.AnyAsync(c => c.Id == countryId);
        }

        public async Task<bool> CountryExists(string countryName)
        {
            return await _context.Countries.AnyAsync(c => c.Name.Trim().ToUpper() == countryName.Trim().ToUpper());
        }

        public async Task<bool> CreateCountry(Country country)
        {
            await _context.Countries.AddAsync(country);
            return await Save();
        }

        public async Task<bool> DeleteCountry(Country country)
        {
            var deleteCountry = await _context.Countries.FirstOrDefaultAsync(c => c.Id == country.Id);
            if (deleteCountry == null) return false;
            _context.Countries.Remove(deleteCountry);
            return await Save();
        }

        public async Task<ICollection<Country>> GetCountries()
        {
            var countries = await _context.Countries.OrderBy(c => c.Name).ToListAsync();
            return countries;
        }

        public async Task<Country>? GetCountry(int countryId)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == countryId);
            return country;
        }

        public async Task<Country>? GetCountryOfAnOwner(int ownerId)
        {
            var country = await _context.Countries
                         .Where(c => c.Owners.Any(o => o.Id == ownerId))
                         .FirstOrDefaultAsync();
            return country;

        }

        public async Task<ICollection<Owner>> GetOwnersByCountry(int countryId)
        {
            var owners = await _context.Owners
                               .Where(o => o.Country.Id == countryId)
                               .OrderBy(o => o.FirstName)
                               .ToListAsync();
            return owners;
        }

        public async Task<bool> Save()
        {
            try
            {
                var saved = await _context.SaveChangesAsync();
                return saved > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateCountry(Country country)
        {
            _context.Update(country);
            return await Save();
        }
    }
}
