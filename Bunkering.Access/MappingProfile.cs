using AutoMapper;
using Bunkering.Core.Data;
using Bunkering.Core.Dtos;
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
            CreateMap<WorkFlow, WorkflowviewModel>().ReverseMap();
            CreateMap<ApplicationRole, RoleViewModel>().ReverseMap();   
            CreateMap<SubmitDocumentDto, SubmittedDocument>().ReverseMap();   
            CreateMap<SubmitDocumentDto, COQSubmittedDocument>().ReverseMap();
            CreateMap<SubmitDocumentDto, PPCOQSubmittedDocument>().ReverseMap();
            CreateMap<ProcessingPlantCOQDTO, ProcessingPlantCOQ>().ReverseMap();
        }
    }
}
