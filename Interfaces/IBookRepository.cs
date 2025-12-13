using dotnet.Models;

namespace dotnet.Interfaces
{
    public interface IBookRepository
    {
        ICollection<Book> GetBooks();
        Book GetBook(int bookId);
        Book GetBook(string bookTitle);
        bool BookExists(int bookId, string bookTitle);
        bool BookExists(int bookId);
        bool CreateBook(int authorId, int genreId, Book book);
        bool UpdateBook(int bookId, int authorId, int genreId, Book book);
        bool Save();
    }
}