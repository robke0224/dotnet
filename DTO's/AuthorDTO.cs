namespace dotnet.DTOs  
{
    public class AuthorDTO
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}