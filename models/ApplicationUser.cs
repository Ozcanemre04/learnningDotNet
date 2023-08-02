using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace dotnet2.models
{
    public class ApplicationUser:IdentityUser
    {   [MinLength(3),MaxLength(15)]
        public string? firstName { get; set;}

        [MinLength(3),MaxLength(15)]
        public string? lastName { get; set;}

        public UserAdress? UserAdress {get; set;}
    }
}