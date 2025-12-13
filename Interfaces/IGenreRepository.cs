using dotnet.Models;

namespace dotnet.Interfaces
{
    public interface IGenreRepository
    {
        ICollection<Genre> GetGenres();
        Genre GetGenre(int genreId);
        ICollection<Book> GetBooksByGenre(int genreId);
        bool GenreExists(int genreId);
        bool CreateGenre(Genre genre);
        bool UpdateGenre(Genre genre);
        bool Save();
    }
}