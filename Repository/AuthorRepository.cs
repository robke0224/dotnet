namespace dotnet.Repository
{
    using dotnet.Data;
    using dotnet.Interfaces;
    using dotnet.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class AuthorRepository : IAuthorRepository
    {
        private readonly DataContext _context;

        public AuthorRepository(DataContext context)
        {
            _context = context;
        }

        public Author GetAuthor(int authorId)
        {
            return _context.Authors.FirstOrDefault(a => a.Id == authorId);
        }

        public ICollection<Author> GetAuthors()
        {
            return _context.Authors.ToList();
        }

        public ICollection<Author> GetAuthorsOfABook(int bookId)
        {
            return _context.BookAuthors
                .Where(ba => ba.BookId == bookId)
                .Select(ba => ba.Author)
                .ToList();
        }

        public ICollection<Book> GetBooksByAuthor(int authorId)
        {
            return _context.BookAuthors
                .Where(ba => ba.AuthorId == authorId)
                .Select(ba => ba.Book)
                .ToList();
        }

        public bool AuthorExists(int authorId)
        {
            return _context.Authors.Any(a => a.Id == authorId);
        }

        public bool CreateAuthor(Author author)
        {
            _context.Add(author);
            return Save();
        }

        public bool UpdateAuthor(Author author)
        {
            if (author == null) return false;

            var existing = _context.Authors.FirstOrDefault(a => a.Id == author.Id);
            if (existing == null) return false;

            existing.FirstName = author.FirstName;
            existing.LastName = author.LastName;

            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
