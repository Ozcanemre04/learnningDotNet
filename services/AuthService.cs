
using dotnet2.data;
using dotnet2.dto;
using dotnet2.models;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace dotnet2.services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;


        public AuthService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager,IConfiguration config){
           _dbContext = dbContext;
           _userManager = userManager;
           _config=config;
        }

        public async Task<RegisterResponseDto> Register(RegisterDto registerDto)
        {   
            var userAlreadyExist = await _userManager.FindByEmailAsync(registerDto.Email);
            
            if(userAlreadyExist != null)
                return new RegisterResponseDto(){
                     IsSucceed=false,
                     Message="email alredy exist"
                };

            var identityUser = new ApplicationUser {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                firstName = registerDto.firstName,
                lastName = registerDto.lastName, 
            };

            var result = await _userManager.CreateAsync(identityUser,registerDto.Password);
            if(!result.Succeeded){
                var errorString = "";
                foreach(var error in result.Errors){
                   errorString += error.Description;
                }
                 return new RegisterResponseDto(){
                     IsSucceed=false,
                     Message=errorString,
                };
            }
          
            return new RegisterResponseDto(){
                     IsSucceed=true,
                     Message="Success, user is created",
                     Email=registerDto.Email,
                     UserName=registerDto.UserName,
                     firstName=registerDto.firstName,
                     lastName=registerDto.lastName,
                };
            
            
        }
        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
            var identity = await _userManager.FindByEmailAsync(loginDto.Email);
            if(identity is null){
                return new AuthResponseDto(){
                    message="user doesnt exist"
                };
            };
           var verifyPassword= await _userManager.CheckPasswordAsync(identity,loginDto.Password);
           if(!verifyPassword){
                return new AuthResponseDto(){
                    message="wrong password"
                };
              };
    
          var claims = new List<Claim>{
                new Claim(ClaimTypes.Email,identity.Email ?? ""),
                new Claim(ClaimTypes.NameIdentifier,identity.Id),
                new Claim(ClaimTypes.Name,identity.UserName ?? ""),
            };
         var token = GenerateToken(claims);
         return new AuthResponseDto(){
            message="Success",
            accessToken=token
         };
    }

        private string GenerateToken(List<Claim> claims)
        {
           
            string key = _config.GetSection("JWT:Key").Value ?? "";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signingCred = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
                claims:claims,
                expires:DateTime.Now.AddMinutes(60),
                issuer:_config.GetSection("JWT:Issuer").Value,
                audience:_config.GetSection("JWT:Audience").Value,
                signingCredentials:signingCred);

            string token=new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }
    }
}