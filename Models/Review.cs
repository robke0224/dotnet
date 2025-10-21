namespace dotnet.Models
{
    public class Review
    {
        public int Id { get; set; }
        public required string BookTitle { get; set; }
        public required string ReviewText { get; set; }
        public Reviewer Reviewer { get; set; }
        public Book Book { get; set; }
    
    }
}