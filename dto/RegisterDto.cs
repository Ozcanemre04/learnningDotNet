using System.ComponentModel.DataAnnotations;

namespace dotnet2.dto
{
    public class RegisterDto
    {
        [Required(ErrorMessage="firstname Is required")]
        [StringLength(50)]
        [MinLength(3)]
        public  string? firstName {get;set;}
        
        [Required(ErrorMessage="author is required")]
        [StringLength(10)]
        [MinLength(3)]
        public  string? lastName {get;set;}

        [Required]
        [EmailAddress]
        public required string Email {get; set;}

        [Required]
        public required string Password {get; set;}

        [Required]
        [StringLength(50)]
        [MinLength(3)]
        public  required string UserName { get; set; }
    }
}