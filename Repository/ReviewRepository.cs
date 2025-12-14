using AutoMapper;
using dotnet.Data;
using dotnet.Interfaces;
using dotnet.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace dotnet.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ReviewRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public bool CreateReview(string reviewerFirstName, string reviewerLastName, Review review)
        {
            if (review == null) return false;

            var title = review.BookTitle.Trim();
            var first = reviewerFirstName.Trim();
            var last = reviewerLastName.Trim();

            var book = _context.Books.FirstOrDefault(b => b.BookTitle == title);
            if (book == null) return false;

            var reviewer = _context.Reviewers.FirstOrDefault(r =>
                r.FirstName.Trim().ToUpper() == first.ToUpper() &&
                r.LastName.Trim().ToUpper() == last.ToUpper());

            if (reviewer == null)
            {
                reviewer = new Reviewer
                {
                    FirstName = first,
                    LastName = last,
                    Reviews = new List<Review>()
                };

                _context.Reviewers.Add(reviewer);
            }

            review.Book = book;
            review.Reviewer = reviewer;

            _context.Reviews.Add(review);
            return Save();
        }

        public bool CreateReview(Review review)
        {
            if (review == null) return false;

            _context.Reviews.Add(review);
            return Save();
        }

        public bool UpdateReview(Review review)
        {
            if (review == null) return false;

            var existing = _context.Reviews
                .Include(r => r.Reviewer)
                .Include(r => r.Book)
                .FirstOrDefault(r => r.Id == review.Id);

            if (existing == null) return false;

            existing.BookTitle = review.BookTitle;
            existing.ReviewText = review.ReviewText;

            if (review.Book != null)
                existing.Book = review.Book;

            if (review.Reviewer != null)
                existing.Reviewer = review.Reviewer;

            return Save();
        }

        
        public bool DeleteReview(Review review)
        {
            if (review == null) return false;

            var existing = _context.Reviews.FirstOrDefault(r => r.Id == review.Id);
            if (existing == null) return false;

            _context.Reviews.Remove(existing);
            return Save();
        }

        public Review GetReview(int reviewId)
        {
            return _context.Reviews
                .Include(r => r.Reviewer)
                .Include(r => r.Book)
                .FirstOrDefault(r => r.Id == reviewId);
        }

        public ICollection<Review> GetReviews()
        {
            return _context.Reviews
                .Include(r => r.Reviewer)
                .Include(r => r.Book)
                .ToList();
        }

        public ICollection<Review> GetReviewsByBook(int bookId)
        {
            return _context.Reviews
                .Include(r => r.Reviewer)
                .Include(r => r.Book)
                .Where(r => r.Book != null && r.Book.Id == bookId)
                .ToList();
        }

        public bool ReviewExists(int reviewId)
        {
            return _context.Reviews.Any(r => r.Id == reviewId);
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
