using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext _context;
        public ReviewerRepository(DataContext context)
        {
            _context = context;
        }
        public ICollection<Reviewer> GetReviewers()
        {
            return _context.Reviewers
                .Include(r => r.Reviews)
                .OrderBy(r => r.Id).ToList();
        }
        public Reviewer? GetReviewer(int reviewerId)
        {
            return _context.Reviewers
                .Include(r => r.Reviews)
                .FirstOrDefault(r => r.Id == reviewerId);
        }
        public bool ReviewerExists(int reviewerId)
        {
            return _context.Reviewers.Any(r => r.Id == reviewerId);
        }

        public ICollection<Review> GetReviewsOfReviewer(int reviewerId)
        {
            return _context.Reviews
                .Where(r => r.Reviewer.Id == reviewerId)
                .ToList();
        }
    }
}