using dotnet2.services;
using Microsoft.AspNetCore.Mvc;
using dotnet2.dto;
namespace dotnet2.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
       private readonly IAuthService _IAuthService;

       public AuthController(IAuthService authService){
         _IAuthService = authService;
       }
       [HttpPost]
       public async Task<ActionResult<RegisterResponseDto>> Register([FromBody] RegisterDto registerDto){
        if(!ModelState.IsValid){
            return BadRequest(ModelState);
        }
        var response = await _IAuthService.Register(registerDto);
        if(response.IsSucceed){
            return Ok(response);
        }
        return BadRequest(response);
       } 
       
       [HttpPost]
       public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto){

         if(!ModelState.IsValid){
            return BadRequest(ModelState);
        }
        var response = await _IAuthService.Login(loginDto);
        if(response.accessToken is null){
            return Unauthorized(response);
        }
        
        return Ok(response);
        
       } 
    }
}