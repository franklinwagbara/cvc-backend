using Bunkering.Access.DAL;
using Bunkering.Core.Data;

namespace Bunkering.Access.IContracts
{
    public interface IUnitOfWork : IDisposable
    {
        IApplication Application { get; }
        IApplicationDepot ApplicationDepot { get; }
        IAppFee AppFee { get; }
        IDepotOfficer DepotOfficer { get; }
        IApplicationType ApplicationType { get; }
        IApplicationHistory ApplicationHistory { get; }
        IAppointment Appointment { get; }
        ICountry Country { get; }
        IDepot Depot { get; }
        IFacility Facility { get; }
        IFacilityType FacilityType { get; }
        IFacilityTypeDocuments FacilityTypeDocuments { get; }
        IInspection Inspection { get; }
        IJetty Jetty { get; }
        ILGA LGA { get; }
        ILocation Location { get; }
        IMessage Message { get; }
        INominatedSurveyor NominatedSurveyor { get; }
        IApplicationSurveyor ApplicationSurveyor { get; }
        IOffice Office { get; }
        IPayment Payment { get; }
        IPermit Permit { get; }
        IProduct Product { get; }
        IRole Role { get; }
        IState State { get; }
        ISubmittedDocument SubmittedDocument { get; }
        ITank Tank { get; }
        IValidatiionResponse ValidatiionResponse { get; }
        IWorkflow Workflow { get; }
        IVesselType VesselType { get; }
        public ICoQ CoQ { get; }
        public ICOQHistory COQHistory { get; }
        public ICOQCertificate COQCertificate { get; }
        IvFacilityPermit vFacilityPermit { get; }
        IvAppPayment vAppPayment { get; }
        IvAppVessel vAppVessel { get; }
        IvAppUser vAppUser { get; }
        IvPayment vPayment { get; }
        int Save();
        Task<int> SaveChangesAsync(string userId);
    }
}
