using dotnet.Data;
using dotnet.Interfaces;
using dotnet.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace dotnet.Repository
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
                .OrderBy(r => r.LastName)
                .ThenBy(r => r.FirstName)
                .ToList();
        }

        public Reviewer GetReviewer(int reviewerId)
        {
            return _context.Reviewers
                .Include(r => r.Reviews)
                    .ThenInclude(rv => rv.Book)
                .Include(r => r.Reviews)
                    .ThenInclude(rv => rv.Reviewer)
                .FirstOrDefault(r => r.Id == reviewerId);
        }


        public ICollection<Review> GetReviewsByReviewer(int reviewerId)
        {
            return _context.Reviews
                .Include(r => r.Book)
                .Include(r => r.Reviewer)
                .Where(r => r.Reviewer != null && r.Reviewer.Id == reviewerId)
                .ToList();
        }

        public bool ReviewerExists(int reviewerId)
        {
            return _context.Reviewers.Any(r => r.Id == reviewerId);
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
            _context.Reviewers.Add(reviewer);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
