using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Models;
using PokemonReviewApp.Services.Interfaces;

namespace PokemonReviewApp.Services.Repository
{
    public class ReviewerService : IReviewerService
    {
        private readonly DataContext _context;
        public ReviewerService(DataContext context)
        {
            _context = context;
        }
        public ICollection<Reviewer> GetReviewers()
        {
            return _context.Reviewers
                //.Include(r => r.Reviews)
                .OrderBy(r => r.Id).ToList();
        }
        public Reviewer? GetReviewer(int reviewerId)
        {
            return _context.Reviewers
                //.Include(r => r.Reviews)
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

        public bool CreateReviewer(Reviewer reviewer)
        {
            _context.Reviewers.Add(reviewer);
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

        public bool UpdateReviewer(Reviewer reviewer)
        {
            _context.Update(reviewer);
            return Save();
        }

        public bool DeleteReviewer(Reviewer reviewer)
        {
            if (reviewer == null) return false;
            var delete = _context.Reviewers.FirstOrDefault(r => r.Id == reviewer.Id);
            if (delete == null) return false;
            _context.Reviewers.Remove(delete);
            return Save();
        }
    }
}