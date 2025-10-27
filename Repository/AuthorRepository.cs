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
            return _context.Authors.Where(a => a.Id == authorId).FirstOrDefault();
        }

        public ICollection<Author> GetAuthorsOfABook(int bookId)
        {
            return _context.BookAuthors.Where(ba => ba.BookId == bookId).Select(ba => ba.Author).ToList();
        }
        
        public ICollection<Author> GetAuthors()
        {
            return _context.Authors.ToList();
        }

        public ICollection<Book> GetBooksByAuthor(int authorId)
        {
            return _context.BookAuthors.Where(ba => ba.AuthorId == authorId).Select(ba => ba.Book).ToList();
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

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}