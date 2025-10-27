namespace dotnet.Helper
{
    using AutoMapper;
    using dotnet.DTOs;
    using dotnet.Models;

    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Book, BookDTO>();
            CreateMap<Genre, GenreDTO>();
            CreateMap<GenreDTO, Genre>();
            CreateMap<Author, AuthorDTO>();
            CreateMap<Review, ReviewDTO>();
            CreateMap<Reviewer, ReviewerDTO>();
        
        }
    }
}