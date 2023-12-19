using Bunkering.Access.IContracts;
using Bunkering.Core.Data;

namespace Bunkering.Access.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationContext _context;
        public IApplication Application { get; private set; }
        public IAppFee AppFee { get; private set; }
        public IDepotOfficer DepotOfficer { get; private set; }
        public IApplicationType ApplicationType { get; set; }
        public IApplicationHistory ApplicationHistory { get; private set; }
        public IAppointment Appointment { get; private set; }
        public ICountry Country { get; private set; }
        public IDepot Depot { get; private set; }
        public IFacility Facility { get; private set; }
        public IFacilityType FacilityType { get; private set; }
        public IFacilityTypeDocuments FacilityTypeDocuments { get; private set; }
        public IOffice Office { get; private set; }
        public IInspection Inspection { get; private set; }
        public IJetty Jetty { get; private set; }
        public ILGA LGA { get; private set; }
        public ILocation Location { get; private set; }
        public IMessage Message { get; private set; }
        public INominatedSurveyor NominatedSurveyor { get; private set; }
        public IPayment Payment { get; private set; }
        public IPermit Permit { get; private set; }
        public IProduct Product { get; private set; }
        public IState State { get; private set; }
        public ISubmittedDocument SubmittedDocument { get; private set; }
        public ITank Tank { get; private set; }
        public IValidatiionResponse ValidatiionResponse { get; set; }
        public IWorkflow Workflow { get; private set; }
        public IVesselType VesselType { get; set; }
        public IvAppVessel vAppVessel { get; private set; }
        public IvAppPayment vAppPayment { get; private set; }
        public IvAppUser vAppUser { get; private set; }
        public IvFacilityPermit vFacilityPermit { get; private set; }
        public IvPayment vPayment { get; private set; }

        public UnitOfWork(ApplicationContext context)
        {
            _context = context;
            Application = Application != null ? Application : new ApplicationRepository(_context);
            AppFee = AppFee != null ? AppFee : new AppFeeRepository(_context);
            ApplicationHistory = ApplicationHistory != null ? ApplicationHistory : new ApplicationHistoryRepository(_context);
            ApplicationType = ApplicationType != null ? ApplicationType : new ApplicationTypeRepository(_context);
            Appointment = Appointment != null ? Appointment : new AppointmentRepository(_context);
            Country = Country != null ? Country : new CountryRepository(_context);
            Depot = Depot != null ? Depot : new DepotRepository(_context);
            Facility = Facility != null ? Facility : new FacilityRepository(_context);
            FacilityType = FacilityType != null ? FacilityType : new FacilityTypeRepository(_context);
            FacilityTypeDocuments = FacilityTypeDocuments != null ? FacilityTypeDocuments : new FacilityTypeDocsRepository(_context);
            Office = Office != null ? Office : new OfficeRepository(_context);
            Inspection = Inspection != null ? Inspection : new InspectionRepository(_context);
            Jetty = Jetty != null ? Jetty : new JettyRepository(_context);
            LGA = LGA != null ? LGA : new LGARepository(_context);
            Location = Location != null ? Location : new LocationRepository(_context);
            Message = Message != null ? Message : new MessageRepository(_context);
            NominatedSurveyor = NominatedSurveyor != null ? NominatedSurveyor : new NominatedSurveyorRepository(_context);
            Payment = Payment != null ? Payment : new PaymentRepository(_context);
            Permit = Permit != null ? Permit : new PermitRepository(_context);
            Product = Product != null ? Product : new ProductRepository(_context);
            State = State != null ? State : new StateRepository(_context);
            SubmittedDocument = SubmittedDocument != null ? SubmittedDocument : new SubmittedDocumentRepository(_context);
            Tank = Tank != null ? Tank : new TankRepository(_context);
            ValidatiionResponse = ValidatiionResponse != null ? ValidatiionResponse : new ValidatiionResponseRepo(_context);
            Workflow = Workflow != null ? Workflow : new WorkflowRepository(_context);
            VesselType = VesselType != null ? VesselType : new VeseelTypeRepository(_context);
            vAppVessel = vAppVessel != null ? vAppVessel : new vAppVesselRepository(_context);
            vFacilityPermit = vFacilityPermit != null ? vFacilityPermit : new vFacilityPermitRepository(_context);
            vAppPayment = vAppPayment != null ? vAppPayment : new vAppPaymentRepository(_context);
            vAppUser = vAppUser != null ? vAppUser : new vAppUserRepository(_context);
            vPayment = vPayment != null ? vPayment : new vPaymentRepository(_context);

        }

        public int Save() => _context.SaveChanges();

        public async Task<int> SaveChangesAsync(string userId) => await _context.SaveChangesAsync(userId);

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
                if (disposing)
                    _context.Dispose();

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
