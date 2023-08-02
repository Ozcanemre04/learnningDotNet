using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using dotnet2.services;
using dotnet2.dto;


namespace dotnet2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

         public ProfileController(IProfileService profileService){
            _profileService = profileService;
         }

        [HttpGet]
        public async Task<IActionResult> getProfile(){
           var profile  =await _profileService.get();
           if(profile.Email is  null)
           {
            return Unauthorized(profile);
           }
           return Ok(profile);
        }
        [HttpPost]
        public async Task<IActionResult> AddProfile([FromBody] AddProfile addProfile){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
           var profile=await _profileService.Add(addProfile);
         
           return CreatedAtAction(nameof(getProfile),profile);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfile updateProfile){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
           var profile=await _profileService.update(updateProfile);
           if(profile.Email == null){
            return NotFound(profile);
           }
         
           return CreatedAtAction(nameof(getProfile),profile);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteProfile(){
           var profile = await _profileService.Delete();
            if(profile == "not found"){
                return NotFound(profile);
            }
            return Ok(profile);
        }
    }
}