namespace dotnet.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public required string GenreName { get; set; }

        public ICollection<BookGenre> BookGenres { get; set; }
    }
    }
