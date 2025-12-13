namespace dotnet.Helper
{
    using AutoMapper;
    using dotnet.DTOs;
    using dotnet.Models;
    using System.Linq;

    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            
            CreateMap<Book, BookDTO>()
                .ForMember(d => d.AuthorFirstName, o => o.MapFrom(s =>
                    s.BookAuthors != null && s.BookAuthors.Any()
                        ? (s.BookAuthors.Select(ba => ba.Author.FirstName).FirstOrDefault() ?? string.Empty)
                        : string.Empty))
                .ForMember(d => d.AuthorLastName, o => o.MapFrom(s =>
                    s.BookAuthors != null && s.BookAuthors.Any()
                        ? (s.BookAuthors.Select(ba => ba.Author.LastName).FirstOrDefault() ?? string.Empty)
                        : string.Empty))
                .ForMember(d => d.GenreName, o => o.MapFrom(s =>
                    s.BookGenres != null && s.BookGenres.Any()
                        ? (s.BookGenres.Select(bg => bg.Genre.GenreName).FirstOrDefault() ?? string.Empty)
                        : string.Empty));

            
            CreateMap<BookDTO, Book>()
                .ForMember(d => d.Reviews, o => o.Ignore())
                .ForMember(d => d.BookAuthors, o => o.Ignore())
                .ForMember(d => d.BookGenres, o => o.Ignore());

            
            CreateMap<Genre, GenreDTO>();
            CreateMap<GenreDTO, Genre>();

           
            CreateMap<Author, AuthorDTO>();
            CreateMap<AuthorDTO, Author>()
                .ForMember(d => d.BookAuthors, o => o.Ignore()); 

            
            CreateMap<Review, ReviewDTO>()
                .ForMember(d => d.ReviewerFirstName, o => o.MapFrom(s =>
                    s.Reviewer != null ? s.Reviewer.FirstName : string.Empty))
                .ForMember(d => d.ReviewerLastName, o => o.MapFrom(s =>
                    s.Reviewer != null ? s.Reviewer.LastName : string.Empty));

            
            CreateMap<ReviewDTO, Review>()
                .ForMember(d => d.Reviewer, o => o.Ignore())
                .ForMember(d => d.Book, o => o.Ignore());

            
CreateMap<Reviewer, ReviewerDTO>()
    .ForMember(d => d.Reviews, o => o.MapFrom(s => s.Reviews));

CreateMap<ReviewerDTO, Reviewer>()
    .ForMember(d => d.Reviews, o => o.Ignore());

        }
    }
}
