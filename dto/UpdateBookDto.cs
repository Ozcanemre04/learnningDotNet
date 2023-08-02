
using System.ComponentModel.DataAnnotations;
using dotnet2.models;
using Mapster;

namespace dotnet2.dto
{
    public class UpdateBookDto
    {
        
        public required  string bookName {get;set;}

        public required  string author {get;set;}

    }
}