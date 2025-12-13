using dotnet.Data;
using dotnet.Interfaces;
using dotnet.Models;
using Microsoft.EntityFrameworkCore;

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
            return _context.Books.Any(b => b.Id == bookId);
        }

        public bool BookExists(int bookId, string bookTitle)
        {
            return _context.Books.Any(b => b.Id == bookId && b.BookTitle == bookTitle);
        }

        public Book GetBook(int bookId)
        {
            return _context.Books
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .Include(b => b.BookGenres)
                    .ThenInclude(bg => bg.Genre)
                .Where(b => b.Id == bookId)
                .FirstOrDefault();
        }

        public Book GetBook(string bookTitle)
        {
            return _context.Books
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .Include(b => b.BookGenres)
                    .ThenInclude(bg => bg.Genre)
                .Where(b => b.BookTitle == bookTitle)
                .FirstOrDefault();
        }

        public ICollection<Book> GetBooks()
        {
            return _context.Books
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .Include(b => b.BookGenres)
                    .ThenInclude(bg => bg.Genre)
                .OrderBy(b => b.BookTitle)
                .ToList();
        }

        public bool CreateBook(int authorId, int genreId, Book book)
        {
            var author = _context.Authors.FirstOrDefault(a => a.Id == authorId);
            var genre = _context.Genres.FirstOrDefault(g => g.Id == genreId);

            if (author == null || genre == null || book == null)
                return false;

            // ensure collections exist (Book has ICollection navs)
            book.BookAuthors ??= new List<BookAuthor>();
            book.BookGenres ??= new List<BookGenre>();
            book.Reviews ??= new List<Review>();

            _context.Books.Add(book);

            var bookAuthor = new BookAuthor
            {
                Author = author,
                Book = book
            };

            var bookGenre = new BookGenre
            {
                Genre = genre,
                Book = book
            };

            _context.Add(bookAuthor);
            _context.Add(bookGenre);

            return _context.SaveChanges() > 0;
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
