
using System.ComponentModel.DataAnnotations;

namespace dotnet2.dto
{
    public class AddBookDto
    {
        [Required(ErrorMessage="the name of book is required")]
        [StringLength(50)]
        [MinLength(3)]
        public  string bookName {get;set;} = null!;
        
        [Required(ErrorMessage="author is required")]
        [StringLength(10)]
        [MinLength(3)]
        public  string author {get;set;} = null!;
    }
}