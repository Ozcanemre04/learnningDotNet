using dotnet2.dto;
using dotnet2.models;

namespace dotnet2.services
{
    public interface IProfileService
    {
        Task<ProfileDto> Add(AddProfile addProfile);
        Task<ProfileDto> update(UpdateProfile updateProfile);
        Task<ProfileDto> get();

        Task<String> Delete();
    }
}