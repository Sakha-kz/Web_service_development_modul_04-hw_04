using AutoMapper;
using BooksApi.Models;

namespace BooksApi.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Book, BookDto>().ReverseMap();
    }
}
