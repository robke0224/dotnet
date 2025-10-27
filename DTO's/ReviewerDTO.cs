namespace dotnet.DTOs
{
    public class ReviewerDTO
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public ICollection<ReviewDTO> Reviews { get; set; }
    }
}