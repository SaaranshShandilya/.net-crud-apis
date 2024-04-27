using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Data;
using TodoApi.Interfaces;
using TodoApi.Models;

namespace TodoApi.Repository
{
    public class OwnerRepository : IOwnerInterface
    {
        private readonly DataContext _context;

        public OwnerRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateOwner(Owner owner)
        {
            _context.Add(owner);
            return Save();
        }

        public Owner GetOwner(int id)
        {
            return _context.Owners.Where(p=>p.Id == id).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerOfPokemon(int id)
        {
            return _context.PokemonOwners.Where(p=>p.Pokemon.Id == id).Select(p=>p.Owner).ToList();
        }

        public ICollection<Owner> GetOwners()
        {
            return _context.Owners.ToList();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int id)
        {
            return _context.PokemonOwners.Where(p=>p.Owner.Id == id).Select(p=>p.Pokemon).ToList();
        }

        public bool OwnerExists(int id)
        {
            return _context.Owners.Any(p=>p.Id == id);
        }

        public bool Save()
        {
           var res = _context.SaveChanges();
           return res>0?true:false;
        }
    }
}