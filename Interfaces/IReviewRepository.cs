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
    }
}