using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Interfaces
{
    public interface IOwnerInterface
    {
        ICollection<Owner> GetOwners();
        Owner GetOwner(int id);
        ICollection<Owner> GetOwnerOfPokemon(int id);
        ICollection<Pokemon> GetPokemonByOwner(int id);
        bool OwnerExists(int id);
        bool CreateOwner(Owner owner);
        bool Save();
    }
}