using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Data;
using TodoApi.Interfaces;
using TodoApi.Models;

namespace TodoApi.Repository
{
    public class ReviewRepository : IReviewInterface
    {
        private readonly  DataContext _context;

        public ReviewRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateReview(Review review)
        {
            _context.Add(review);
            return Save();
        }

        public Review GetReview(int id)
        {
            return _context.Reviews.Where(r=>r.Id==id).FirstOrDefault();   
        }

        public ICollection<Review> GetReviews()
        {
            return _context.Reviews.ToList();
        }

        public ICollection<Review> GetReviewsOfPokemon(int id)
        {
            return _context.Reviews.Where(r=>r.Pokemon.Id == id).ToList();
        }

        public bool ReviewExists(int id)
        {
            return _context.Reviews.Any(r=>r.Id == id);
        }

        public bool Save()
        {
            var res = _context.SaveChanges();
            return res>0?true:false;
        }
    }
}