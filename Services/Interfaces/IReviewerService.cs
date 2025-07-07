using PokemonReviewApp.Models;

namespace PokemonReviewApp.Services.Interfaces
{
    public interface IReviewerService
    {
        ICollection<Reviewer> GetReviewers();
        Reviewer? GetReviewer(int reviewerId);
        ICollection<Review> GetReviewsOfReviewer(int reviewerId);
        bool ReviewerExists(int reviewerId);
        bool CreateReviewer(Reviewer reviewer);
        bool UpdateReviewer(Reviewer reviewer);
        bool DeleteReviewer(Reviewer reviewer);
        bool Save();
    }
}
