﻿using PokemonReviewApp.Models;

namespace PokemonReviewApp.Services.Interfaces
{
    public interface IPokemonService
    {
        ICollection<Pokemon> GetPokemons();
        Pokemon? GetPokemon(int id);
        Pokemon? GetPokemon(string name);
        decimal GetPokemonRating(int pokeid);
        bool PokemonExists(int id);
        bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon);
        bool UpdatePokemon(Pokemon pokemon);
        bool DeletePokemon(Pokemon pokemon);
        bool Save();
    }
}
