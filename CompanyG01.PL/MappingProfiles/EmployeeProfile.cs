using AutoMapper;
using CompanyG01.DAL.Models;
using CompanyG01.PL.ViewModels;

namespace CompanyG01.PL.MappingProfiles
{
    public class EmployeeProfile: Profile
    {
        public EmployeeProfile()
        {
            //CreateMap<EmployeeViewModel, Employee>()
            //    .ForMember(D => D.Name, O => O
            //    .MapFrom(S => S.EmpName));
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
        }
    }
}
