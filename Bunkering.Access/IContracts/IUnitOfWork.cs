using Bunkering.Access.DAL;
using Bunkering.Core.Data;

namespace Bunkering.Access.IContracts
{
    public interface IUnitOfWork : IDisposable
    {
        IApplication Application { get; }
        IApplicationDepot ApplicationDepot { get; }
        IAppFee AppFee { get; }
        IPantOfficer PlantOfficer { get; }
        IApplicationType ApplicationType { get; }
        IApplicationHistory ApplicationHistory { get; }
        IAppointment Appointment { get; }
        ICountry Country { get; }
        IDepot Depot { get; }
        IEmailConfiguration EmailConfiguration { get; }
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
        IPlant Plant { get; }
        IPlantTank PlantTank { get; }
        IState State { get; }
        ISubmittedDocument SubmittedDocument { get; }
        ITank Tank { get; }
        IValidatiionResponse ValidatiionResponse { get; }
        IWorkflow Workflow { get; }
        IVesselType VesselType { get; }
        ICoQ CoQ { get; }
        ICOQTank CoQTank { get; }
        ICOQHistory COQHistory { get; }
        ICOQCertificate COQCertificate { get; }
        IMeasurementTypeRepository MeasurementType { get; }
        IvFacilityPermit vFacilityPermit { get; }
        IvAppPayment vAppPayment { get; }
        IvAppVessel vAppVessel { get; }
        IvAppUser vAppUser { get; }
        IvPayment vPayment { get; }
        IvDebitNote vDebitNote { get; }
        int Save();
        Task<int> SaveChangesAsync(string userId);
    }
}
