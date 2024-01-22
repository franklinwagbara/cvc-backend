using AutoMapper;
using Bunkering.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<ApplictionViewModel, Application>().ReverseMap();
			CreateMap<AppDepotViewModel, ApplicationDepot>().ReverseMap();
			CreateMap<TankViewModel, Tank>().ReverseMap();
			CreateMap<WorkflowviewModel, WorkFlow>().ReverseMap();
			CreateMap<FacilitySourceDto, FacilitySource>().ReverseMap();
			CreateMap<CreateCoQGasTankDTO, TankMeasurement>().ReverseMap();
			CreateMap<CreateCoQLiquidDto, TankMeasurement>().ReverseMap();
		}
	}
}
