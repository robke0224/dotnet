using dotnet.Data;
using dotnet.Interfaces;
using dotnet.Models;

namespace dotnet.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly DataContext _context;
        public BookRepository(DataContext context)
        {
            _context = context;
    }

        public bool BookExists(int bookId)
        {
            throw new NotImplementedException();
        }

        public Book GetBook(int bookId)
        {
            return _context.Books.Where(b => b.Id == bookId).FirstOrDefault();
        }

        public Book GetBook(string bookTitle)
        {
            return _context.Books.Where(b => b.BookTitle == bookTitle).FirstOrDefault();
        }

        public ICollection<Book> GetBooks()
        {
            return _context.Books.OrderBy(b => b.BookTitle).ToList();
        }
    
        public bool BookExists(int bookId, string bookTitle)
        {
            return _context.Books.Any(b => b.Id == bookId && b.BookTitle == bookTitle);
        }
}
}