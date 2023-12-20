using AutoMapper;
using Bunkering.Core.Data;
using Bunkering.Core.ViewModels;

namespace Bunkering.Access
{
    public class MappingProfile : Profile
    {
        protected MappingProfile()
        {
            CreateMap<CoQ, CoQViewModel>().ReverseMap();
        }
    }
}
