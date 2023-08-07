
using dotnet2.dto;
using Microsoft.AspNetCore.Identity;
namespace dotnet2.services
{
    public interface IAuthService
    {
        Task<RegisterResponseDto> Register(RegisterDto registerDto);
        Task<AuthResponseDto> Login(LoginDto loginDto);

        Task<string> refreshToken();
        Task<string> logout();

    }
}