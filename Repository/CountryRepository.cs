using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Data;
using TodoApi.Interfaces;
using TodoApi.Models;

namespace TodoApi.Repository
{
    public class CountryRepository : ICountryInterface
    {
        private readonly DataContext _context;
        public CountryRepository(DataContext context)
        {
            _context = context;
        }
        public bool CountryExists(int id)
        {
            return _context.Countries.Any(c=>c.Id == id);
        }

        public bool CreateCountry(Country country)
        {
            _context.Add(country);
            return Save();
        }

        public ICollection<Country> GetCountries()
        {
            return _context.Countries.ToList();
        }

        public Country GetCountry(int Id)
        {
            return _context.Countries.Where(c=>c.Id == Id).FirstOrDefault();
        }

        public Country GetCountryByOwner(int ownerId)
        {
            return _context.Owners.Where(u=>u.Id == ownerId).Select(c=>c.Country).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnersFromCountry(int id)
        {
            return _context.Owners.Where(o=>o.Country.Id == id).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved>0?true:false;
        }

        public bool UpdateCountry(Country country)
        {
            _context.Update(country);
            return Save(); 
        }
    }
}