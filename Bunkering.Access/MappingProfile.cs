using AutoMapper;
using Bunkering.Core.Data;
using Bunkering.Core.ViewModels;

namespace Bunkering.Access
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CoQ, CoQViewModel>().ReverseMap();
            CreateMap<CoQ, CreateCoQViewModel>().ReverseMap();
            CreateMap<ApplicationDepot, AppDepotViewModel>().ReverseMap();
        }
    }
}
