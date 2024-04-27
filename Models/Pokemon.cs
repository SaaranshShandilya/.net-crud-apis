using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Models
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public ICollection<Review> Reviews { get; set; } //one to many relation
        public ICollection<PokemonOwner> PokemonOwners { get; set; } //many to many relation
        public ICollection<PokemonCategory> PokemonCategories { get; set; }

    }
}