
namespace dotnet2.dto
{
    public class RegisterResponseDto
    {
        public bool IsSucceed { get; set; }
        public required string Message { get; set; }
        
        public string? UserName { get; set; }
        public string? Email {get; set;}
        public  string? firstName {get;set;}
        
        public  string? lastName {get;set;}


    }
}