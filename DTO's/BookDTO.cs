namespace dotnet.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; }
        public required string BookTitle { get; set; }
        public required int BookPublicationDate { get; set; }
    }
}