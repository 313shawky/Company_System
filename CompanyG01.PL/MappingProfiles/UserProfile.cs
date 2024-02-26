using AutoMapper;
using AutoMapper.Configuration;
using CompanyG01.DAL.Models;
using CompanyG01.PL.ViewModels;

namespace CompanyG01.PL.MappingProfiles
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserViewModel>().ReverseMap();
        }
    }
}
