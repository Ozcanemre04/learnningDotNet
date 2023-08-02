
namespace dotnet2.dto
{
    public class BookDto
    {
        public Guid Id {get;set;}

        public required string bookName {get;set;}

        public required string author {get;set;}
    }
}