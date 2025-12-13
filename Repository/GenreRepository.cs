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
            return _context.Genres.FirstOrDefault(g => g.Id == genreId);
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
            _context.Add(genre);
            return Save();
        }

       
        public bool UpdateGenre(Genre genre)
        {
            var existing = _context.Genres.FirstOrDefault(g => g.Id == genre.Id);
            if (existing == null) return false;

            existing.GenreName = genre.GenreName;

            return Save();
        }


        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
