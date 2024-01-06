using AutoMapper;
using NewZealandWalks.Models.Domain;
using NewZealandWalks.Models.DTOs;

namespace NewZealandWalks.Mappings
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Region,RegionDTO>().ReverseMap();
        }
    }
}
