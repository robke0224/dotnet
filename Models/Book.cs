namespace dotnet.Models
{
    public class Book
    {
        public int Id { get; set; }
        public required string BookTitle { get; set; }
        public required int BookPublicationDate { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<BookAuthor> BookAuthors { get; set; }
        public ICollection<BookGenre> BookGenres { get; set; }

    }
}