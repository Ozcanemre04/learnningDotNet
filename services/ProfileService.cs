using Microsoft.AspNetCore.Http;
using dotnet2.data;
using dotnet2.dto;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using dotnet2.models;
using Microsoft.EntityFrameworkCore;

namespace dotnet2.services
{
    public class ProfileService : IProfileService
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
         public ProfileService(ApplicationDbContext dbContext,IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager){
            _dbcontext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
         }
        private string currentUser(){
            string User = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return User;

        }
        public async Task<ProfileDto> Add(AddProfile addProfile)
        {
            var userId = currentUser();
            var user  = await _userManager.FindByIdAsync(userId);
            var useradress = await _dbcontext.Adress.SingleOrDefaultAsync(x=>x.UserId == userId);
            if(useradress is not null){
                 throw new Exception("adress is already added");
            }
            var profile = new UserAdress {
                Country = addProfile.Country,
                City = addProfile.City,
                Address = addProfile.Address,
                State = addProfile.State,
                UserId = userId,
                ApplicationUser = user
            };
            _dbcontext.Adress.Add(profile);
            await _dbcontext.SaveChangesAsync();
            var profileDto = new ProfileDto {
                   message = "SUCCESS,USER ADRESS IS ADDED",
                   UserId = user?.Id,
                   firstName=user?.firstName,
                   lastName=user?.lastName,
                   Email=user?.Email,
                   UserName=user?.UserName,
                   Adress= new AdressDto {
                   State = profile.State,
                   Country = profile.Country,
                   City = profile.City,
                   Address = profile.Address,  
                   } 
            };
            return profileDto;
        }


        public async Task<ProfileDto> get()
        {
            var UserId =  currentUser();
            if(UserId is not null){
            var user  = await _userManager.FindByIdAsync(UserId);
            var useradress = await _dbcontext.Adress.Include(a=>a.ApplicationUser).SingleOrDefaultAsync(x=>x.UserId == UserId);
            var userDto = new ProfileDto 
                 {
                   message= "success",
                   UserId = user.Id,
                   firstName=user.firstName,
                   lastName=user.lastName,
                   Email=user.Email,
                   UserName=user.UserName,
                   Adress= new AdressDto {
                   State = useradress?.State,
                   Country = useradress?.Country,
                   City = useradress?.City,
                   Address = useradress?.Address,  
                   }
                 };
            return userDto;

            }
    
            var Dto = new ProfileDto {};
            return Dto;           
        }

        public async Task<ProfileDto> update(UpdateProfile updateProfile)
        {
            var userId = currentUser();
            var user  = await _userManager.FindByIdAsync(userId);
            var useradress = await _dbcontext.Adress.Include(a=>a.ApplicationUser).SingleOrDefaultAsync(x=>x.UserId == userId);
            if(useradress is null) {
                user.firstName = updateProfile.firstName;
                user.lastName = updateProfile.lastName;
                await _userManager.UpdateAsync(user);

                return new ProfileDto{
                   message="user updated but user address doesnt exist",
                   UserId = user.Id,
                   firstName=user.firstName,
                   lastName=user.lastName,
                   Email=user.Email,
                   UserName=user.UserName,  
                };
            }
            useradress.ApplicationUser.firstName = updateProfile.firstName;
            useradress.ApplicationUser.lastName = updateProfile.lastName;
            useradress.Country = updateProfile.Adress.Country;
            useradress.City = updateProfile.Adress.City;
            useradress.State = updateProfile.Adress.State;
            useradress.Address = updateProfile.Adress.Address;
            await _dbcontext.SaveChangesAsync();
            var profileDto = new ProfileDto {
                   message= "success,User and user adress are upated",
                   UserId = useradress.ApplicationUser.Id,
                   firstName=useradress.ApplicationUser.firstName,
                   lastName=useradress.ApplicationUser.lastName,
                   Email=useradress.ApplicationUser.Email,
                   UserName=useradress.ApplicationUser.UserName,
                   Adress= new AdressDto {
                   State = useradress?.State,
                   Country = useradress?.Country,
                   City = useradress?.City,
                   Address = useradress?.Address,  
                   }
            };
            return profileDto;
        }

        public async Task<string> Delete()
        {
            var userId = currentUser();
            var useradress = await _dbcontext.Adress.SingleOrDefaultAsync(x=>x.UserId == userId);

            
            if(useradress == null){
                string errorMessage = "not found";
                 return errorMessage;
            } 
            _dbcontext.Remove(useradress);
            await _dbcontext.SaveChangesAsync();
            var deletedMessage = "adress deleted";
            return deletedMessage;
        }
    }
}