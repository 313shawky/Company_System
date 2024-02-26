using AutoMapper;
using CompanyG01.PL.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace CompanyG01.PL.MappingProfiles
{
    public class RoleProfile: Profile
    {
        public RoleProfile()
        {
            CreateMap<IdentityRole, RoleViewModel>()
                .ForMember(D => D.RoleName, O => O.MapFrom(S => S.Name))
                .ReverseMap();
        }
    }
}
