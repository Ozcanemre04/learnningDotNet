
using System.ComponentModel.DataAnnotations;

namespace dotnet2.dto
{
    public class AddBookDto
    {
        
        public  string bookName {get;set;} = null!;
          
        public  string author {get;set;} = null!;
    }
}