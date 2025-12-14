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
                .Include(b => b.Reviews)
                .FirstOrDefault(b => b.Id == bookId);
        }

      
        public Book GetBook(string bookTitle)
        {
            if (string.IsNullOrWhiteSpace(bookTitle))
                return null;

            var title = bookTitle.Trim().ToUpper();

            return _context.Books
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .Include(b => b.BookGenres)
                    .ThenInclude(bg => bg.Genre)
                .Include(b => b.Reviews)
                .FirstOrDefault(b => b.BookTitle != null && b.BookTitle.Trim().ToUpper() == title);
        }

        public ICollection<Book> GetBooks()
        {
            return _context.Books
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .Include(b => b.BookGenres)
                    .ThenInclude(bg => bg.Genre)
                .Include(b => b.Reviews)
                .OrderBy(b => b.BookTitle)
                .ToList();
        }

        public bool CreateBook(int authorId, int genreId, Book book)
        {
            var author = _context.Authors.FirstOrDefault(a => a.Id == authorId);
            var genre = _context.Genres.FirstOrDefault(g => g.Id == genreId);

            if (author == null || genre == null || book == null)
                return false;

            book.BookAuthors ??= new List<BookAuthor>();
            book.BookGenres ??= new List<BookGenre>();
            book.Reviews ??= new List<Review>();

            _context.Books.Add(book);

            _context.Add(new BookAuthor { Author = author, Book = book });
            _context.Add(new BookGenre { Genre = genre, Book = book });

            return _context.SaveChanges() > 0;
        }

        public bool UpdateBook(int bookId, int authorId, int genreId, Book book)
        {
            var existing = _context.Books
                .Include(b => b.BookAuthors)
                .Include(b => b.BookGenres)
                .FirstOrDefault(b => b.Id == bookId);

            if (existing == null) return false;

            var author = _context.Authors.FirstOrDefault(a => a.Id == authorId);
            var genre = _context.Genres.FirstOrDefault(g => g.Id == genreId);

            if (author == null || genre == null || book == null) return false;

            existing.BookTitle = book.BookTitle;
            existing.BookPublicationDate = book.BookPublicationDate;

            if (existing.BookAuthors != null && existing.BookAuthors.Any())
                _context.BookAuthors.RemoveRange(existing.BookAuthors);

            if (existing.BookGenres != null && existing.BookGenres.Any())
                _context.BookGenres.RemoveRange(existing.BookGenres);

            _context.BookAuthors.Add(new BookAuthor { Book = existing, Author = author });
            _context.BookGenres.Add(new BookGenre { Book = existing, Genre = genre });

            return _context.SaveChanges() > 0;
        }

        
        public bool DeleteBook(Book book)
        {
            if (book == null) return false;

            var existing = _context.Books
                .Include(b => b.BookAuthors)
                .Include(b => b.BookGenres)
                .Include(b => b.Reviews)
                .FirstOrDefault(b => b.Id == book.Id);

            if (existing == null) return false;

            if (existing.Reviews != null && existing.Reviews.Any())
                _context.Reviews.RemoveRange(existing.Reviews);

            if (existing.BookAuthors != null && existing.BookAuthors.Any())
                _context.BookAuthors.RemoveRange(existing.BookAuthors);

            if (existing.BookGenres != null && existing.BookGenres.Any())
                _context.BookGenres.RemoveRange(existing.BookGenres);

            _context.Books.Remove(existing);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
