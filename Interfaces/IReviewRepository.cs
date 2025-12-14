namespace dotnet.Interfaces
{
    using dotnet.Models;
    using System.Collections.Generic;

    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int reviewId);
        ICollection<Review> GetReviewsByBook(int bookId);
        bool ReviewExists(int reviewId);

        bool CreateReview(Review review);

        bool CreateReview(string reviewerFirstName, string reviewerLastName, Review review);
        bool UpdateReview(Review review);
        bool DeleteReview(Review review);

        bool Save();
    }
}
