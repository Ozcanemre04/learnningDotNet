using System.Xml;


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet2.models
{ 
    [Table("books")]
    public class Books
    {   
        [Key]
        public Guid Id {get;set;}

        [Required(ErrorMessage="the name of book is required")]
        [StringLength(50)]
        public required string bookName {get;set;}

        [Required(ErrorMessage="author is required")]
        [StringLength(10)]
        public required string author {get;set;}
    }
}