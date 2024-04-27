using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Data;
using TodoApi.Interfaces;
using TodoApi.Models;

namespace TodoApi.Repository
{
    public class PokemonRepository :  IPokemonInterface
    {
        private readonly DataContext _context;
        public PokemonRepository(DataContext context){
            _context = context;
        }

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var pokemonEntity = _context.Owners.Where(p=>p.Id == ownerId).FirstOrDefault();
            var category  = _context.Categories.Where(a=>a.Id == categoryId).FirstOrDefault();
            
            var pokemonOwner = new PokemonOwner(){
                Owner = pokemonEntity,
                Pokemon = pokemon
            };
            _context.Add(pokemonOwner);
            var pokemonCategory = new PokemonCategory(){
                Category = category,
                Pokemon  = pokemon
            };
            _context.Add(pokemonCategory);
            _context.Add(pokemon);
            return Save();
            
        }

        public Pokemon GetPokemon(int id)
        {
            return _context.Pokemons.Where(p => (p.Id == id)).FirstOrDefault();
        }

        public Pokemon GetPokemon(string name)
        {
            return _context.Pokemons.Where(p=>p.Name == name).FirstOrDefault();
        }

        public decimal GetPokemonRating(int id)
        {
            var r = _context.Reviews.Where(p=>p.Pokemon.Id == id);
            if(r.Count() <= 0){
                return 0;
            }
            return (decimal)r.Sum(r=>r.Rating)/r.Count();
        }

        public ICollection<Pokemon> GetPokemons(){
            return _context.Pokemons.OrderBy(p=>p.Id).ToList();
        }

        public bool PokemonExist(int PokeId)
        {
            return _context.Pokemons.Any(p => p.Id ==PokeId);
        }

        public bool Save()
        {
            var res = _context.SaveChanges();
            return res>0?true:false;
        }

        public bool UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            _context.Update(pokemon);
            return Save();
        }
    }
}