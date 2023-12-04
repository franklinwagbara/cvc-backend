﻿using Bunkering.Access.DAL;

namespace Bunkering.Access.IContracts
{
	public interface IUnitOfWork : IDisposable
	{
		IApplication Application { get; }
		IAppFee AppFee { get; }
		IApplicationType ApplicationType { get; }
		IApplicationHistory ApplicationHistory { get; }
		IAppointment Appointment { get; }
		ICountry Country { get; }
		IFacility Facility { get; }
		IFacilityType FacilityType { get; }
		IFacilityTypeDocuments FacilityTypeDocuments { get; }
		IInspection Inspection { get; }
		ILGA LGA { get; }
		ILocation Location { get; }
		IMessage Message { get; }
		IOffice Office { get; }
		IPayment Payment { get; }
		IPermit Permit { get; }
		IProduct Product { get; }
		IState State { get; }
		ISubmittedDocument SubmittedDocument { get; }
		ITank Tank { get; }
		IValidatiionResponse ValidatiionResponse { get; }
		IWorkflow Workflow { get; }
		IVesselType VesselType { get; }
		IvFacilityPermit vFacilityPermit { get; }
		IvAppPayment vAppPayment { get; }
		IvAppVessel vAppVessel { get; }
		IvAppUser vAppUser { get; }
		int Save();
		Task<int> SaveChangesAsync(string userId);
	}
}
