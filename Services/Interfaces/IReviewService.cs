using PokemonReviewApp.Models;

namespace PokemonReviewApp.Services.Interfaces
{
    public interface IReviewService
    {
        ICollection<Review> GetReviews();
        Review? GetReview(int reviewId);
        ICollection<Review> GetReviewsOfAPokemon(int pokeId);
        bool CreateReview(Review review);
        bool UpdateReview(Review review);
        bool DeleteReview(Review review);
        bool Save();
    }
}
