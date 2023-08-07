
using dotnet2.data;
using dotnet2.dto;
using dotnet2.models;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace dotnet2.services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IHttpContextAccessor _httpContextAccessor;



        public AuthService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager,IConfiguration config,IHttpContextAccessor httpContextAccessor){
           _dbContext = dbContext;
           _userManager = userManager;
           _config=config;
           _httpContextAccessor = httpContextAccessor;
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
            var refresh = await _dbContext.RefreshToken.FirstOrDefaultAsync(x=>x.UserId == identity.Id);
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
    
          if(identity.RefreshToken != null ){
            var token0 = GenerateToken(identity);
            
            refresh.refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(100));
            refresh.expires = DateTime.Now.AddDays(7);
            await _dbContext.SaveChangesAsync();
            SetRefreshToken(refresh);
            return new AuthResponseDto(){
            message="Success",
            accessToken=token0,
         };
          }
         var token = GenerateToken(identity);
         var refreshToken = new RefreshToken{
                refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(100)),
                expires = DateTime.Now.AddDays(7),
                UserId = identity.Id,
                ApplicationUser = identity

            };
            _dbContext.RefreshToken.Add(refreshToken);
            await _dbContext.SaveChangesAsync();
            SetRefreshToken(refreshToken);
         return new AuthResponseDto(){
            message="Success",
            accessToken=token,
         };
    }   
       public async Task<string> refreshToken()
        {
            var GetRefreshtoken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
            var userid  = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user  = await _userManager.FindByIdAsync(userid);
            var refresh = await _dbContext.RefreshToken.FirstOrDefaultAsync(x=>x.UserId == userid);
            if(refresh?.refreshToken == null){
                  string msg = "refresh token is null";
                  return msg;
            }
            if(!refresh.refreshToken.Equals(GetRefreshtoken)){
                string msg = "Invalid refresh Token";
                return msg;
            }
            else if(refresh?.expires < DateTime.Now){
                 string msg = "Token expired";
                return msg;
            }
            string token  = GenerateToken(user);
            refresh.refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(100));
            refresh.expires = DateTime.Now.AddDays(7);
            await _dbContext.SaveChangesAsync();
            SetRefreshToken(refresh);
            return token;

        }
        public async Task<string> logout()
        {   
            var userid  = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user  = await _userManager.FindByIdAsync(userid);
            var refresh = await _dbContext.RefreshToken.FirstOrDefaultAsync(x=>x.UserId == userid);
            if(user.RefreshToken == null){
                var errorMessage  = "cookie not found";
                return errorMessage;
            }
             _dbContext.Remove(refresh);
             await _dbContext.SaveChangesAsync();
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("refreshToken");
            var msg  = "cookie deleted";
            return msg;
           
        }
        
        private void SetRefreshToken(RefreshToken newToken){
            var cookieOptions = new CookieOptions{
                HttpOnly = true,
                Expires= newToken.expires,
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken",newToken.refreshToken, cookieOptions);
          
        }
        private string GenerateToken(ApplicationUser user)
        {
           
              var claims = new List<Claim>{
                new Claim(ClaimTypes.Email,user.Email ?? ""),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Name,user.UserName ?? ""),
            };

            string key = _config.GetSection("JWT:Key").Value ?? "";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signingCred = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
                claims:claims,
                expires:DateTime.Now.AddMinutes(1),
                issuer:_config.GetSection("JWT:Issuer").Value,
                audience:_config.GetSection("JWT:Audience").Value,
                signingCredentials:signingCred);

            string token=new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }

        
    }
}