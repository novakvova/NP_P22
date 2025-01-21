using _6.NovaPoshta.Data.Entities;
using _6.NovaPoshta.Models.Area;
using _6.NovaPoshta.Models.City;
using _6.NovaPoshta.Models.Department;
using AutoMapper;

namespace _6.NovaPoshta.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AreaItemResponse, AreaEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Cities, opt => opt.Ignore());
            CreateMap<CityItemResponse, CityEntity>()
                .ForMember(dest => dest.AreaId, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Area, opt => opt.Ignore())
                .ForMember(dest => dest.Departments, opt => opt.Ignore())
                .ForMember(dest => dest.AreaRef, opt => opt.MapFrom(src => src.Area))
                .ForMember(dest => dest.TypeDescription, opt => opt.MapFrom(src => src.SettlementTypeDescription));
            CreateMap<DepartmentItemResponse, DepartmentEntity>()
                .ForMember(dest => dest.CityId, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.City, opt => opt.Ignore())
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.ShortAddress));
        }
    }
}
