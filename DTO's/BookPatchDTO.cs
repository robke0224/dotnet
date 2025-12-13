namespace dotnet.DTOs
{
    public class BookPatchDTO
    {
        public string? BookTitle { get; set; }
        public int? BookPublicationDate { get; set; }

        public string? AuthorFirstName { get; set; }
        public string? AuthorLastName { get; set; }

        public string? GenreName { get; set; }
    }
}
