using AutoMapper;
using CompanyG01.DAL.Models;
using CompanyG01.PL.ViewModels;

namespace CompanyG01.PL.MappingProfiles
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<DepartmentViewModel, Department>().ReverseMap();
        }
    }
}
