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
        }
    }
}