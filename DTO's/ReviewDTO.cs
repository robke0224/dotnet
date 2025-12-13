namespace dotnet.DTOs
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public required string BookTitle { get; set; }
        public required string ReviewText { get; set; }
        public required string ReviewerFirstName { get; set; }
        public required string ReviewerLastName { get; set; }
    }
}