﻿using AutoMapper;
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
			CreateMap<WorkflowviewModel, WorkFlow>()
				.ForMember(w => w.TriggeredByRole, opt => opt.MapFrom(src => src.TriggeredByRoleId))
                .ForMember(w => w.TargetRole, opt => opt.MapFrom(src => src.TargetRoleId)).ReverseMap();
            CreateMap<FacilitySourceDto, FacilitySource>().ReverseMap();
			CreateMap<TankMeasurement, CreateCoQGasTankDTO>().ReverseMap()
				.ForMember(t => t.Tempearture, opt =>
			{
				opt.PreCondition(pr => (pr.LiquidTemperature > 0));
				opt.MapFrom(src => src.LiquidTemperature);
            });
			CreateMap<TankMeasurement, CreateCoQLiquidTankDto>().ReverseMap()
				.ForMember(t => t.Tempearture, opt => opt.MapFrom(src => src.Temperature));
		}
	}
}
