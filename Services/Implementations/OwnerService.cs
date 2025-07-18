﻿using AutoMapper;
using PokemonReviewApp.Data;
using PokemonReviewApp.Models;
using PokemonReviewApp.Services.Interfaces;

namespace PokemonReviewApp.Services.Repository
{
    public class OwnerService : IOwnerService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public OwnerService(DataContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public ICollection<Owner> GetOwners()
        {
            return _context.Owners
                .OrderBy(o => o.FirstName)
                .ToList();
        }
        public Owner? GetOwner(int ownerId)
        {
            return _context.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerOfAPokemon(int pokeId)
        {
            return _context.PokemonOwners
                .Where(po => po.PokemonId == pokeId)
                .Select(po => po.Owner)
                .ToList();
        }

        public ICollection<Pokemon> GetPokemonsByOwner(int ownerId)
        {
            return _context.PokemonOwners
                .Where(po => po.OwnerId == ownerId)
                .Select(po => po.Pokemon)
                .ToList();
        }

        public bool OwnerExists(int ownerId)
        {
            return _context.Owners.Any(o => o.Id == ownerId);
        }

        public bool CreateOwner(Owner owner)
        {
            //var countryIdEntity = _context.Countries.FirstOrDefault(c => c.Id == countryId);
            //var country = new Country()
            //{
            //    Id = countryIdEntity.Id,
            //    Name = countryIdEntity.Name
            //};
            //_context.Countries.Add(country);
            _context.Owners.Add(owner);
            return Save();
        }

        public bool Save()
        {
            try
            {
                var saved = _context.SaveChanges();
                return saved > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Save error: {ex.Message}");
                Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");
                return false;
            }
        }

        public bool UpdateOwner(Owner owner)
        {
            _context.Update(owner);
            return Save();
        }

        public bool DeleteOwner(Owner owner)
        {
            if (owner == null)
            {
                return false;
            }
            var delete = _context.Owners.FirstOrDefault(o => o.Id == owner.Id);
            if (delete == null)
            {
                return false;
            }
            _context.Remove(delete);
            return Save();
        }
    }
}
