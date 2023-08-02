using System.ComponentModel.DataAnnotations;

namespace dotnet2.dto
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public required string Email {get; set;}

        [Required]
        public required string Password {get; set;}
    }
}