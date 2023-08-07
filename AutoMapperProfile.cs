using AutoMapper;
using dotnet2.dto;
using dotnet2.models;

namespace dotnet2
{
    public class AutoMapperProfile :Profile
    {
        public AutoMapperProfile(){
            CreateMap<Books,BookDto>().ReverseMap();
            CreateMap<AddBookDto,Books>();
            CreateMap<UpdateBookDto,Books>();
        
        }
        
    }
}