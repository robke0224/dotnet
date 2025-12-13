namespace dotnet.Repository 
{
    using dotnet.Data;
    using dotnet.Interfaces;
    using dotnet.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;

    public class GenreRepository : IGenreRepository
    {
        private readonly DataContext _context;

        public GenreRepository(DataContext context)
        {
            _context = context;
        }

        public bool GenreExists(int genreId)
        {
            return _context.Genres.Any(g => g.Id == genreId);
        }

        public ICollection<Genre> GetGenres()
        {
            return _context.Genres.ToList();
        }

        public Genre GetGenre(int genreId)
        {
            return _context.Genres.Where(g => g.Id == genreId).FirstOrDefault();
        }

       public ICollection<Book> GetBooksByGenre(int genreId)
{
    return _context.Books
                   .Include(b => b.BookGenres)
                   .Where(b => b.BookGenres != null && b.BookGenres.Any(bg => bg.GenreId == genreId))
                   .ToList();
}

        public bool CreateGenre(Genre genre)
        { 
            //change tracker
            _context.Add(genre);
            return Save();
        }
 
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}