using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _context;

        public ReviewRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Review> GetReviews()
        {
            return _context.Reviews.ToList();
        }

        public Review? GetReview(int reviewId)
        {
            return _context.Reviews.FirstOrDefault(r => r.Id == reviewId);
        }

        public ICollection<Review> GetReviewsOfAPokemon(int pokeId)
        {
            return _context.Reviews
                .Where(r => r.Pokemon.Id == pokeId)
                .ToList();
        }

        public bool CreateReview(Review review)
        {
            _context.Reviews.Add(review);
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

        public bool UpdateReview(Review review)
        {
            _context.Update(review);
            return Save();
        }

        public bool DeleteReview(Review review)
        {
            if (review == null) return false;
            _context.Reviews.Remove(review);
            return Save();
        }
    }
}
