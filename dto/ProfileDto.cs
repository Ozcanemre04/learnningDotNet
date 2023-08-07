
using dotnet2.dto;
namespace dotnet2.dto
{
    public class ProfileDto
    {   
        public string ? message {get;set;}
        public string? UserId {get;set;}
        public string? Email {get; set;}
        public  string? UserName { get; set; }
        public  string? firstName {get;set;}
        public  string? lastName {get;set;}
      
        public AdressDto? Adress{ get; set; }

       
    }
}