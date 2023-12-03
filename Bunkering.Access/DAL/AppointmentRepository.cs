using Bunkering.Access.IContracts;
using Bunkering.Core.Data;


namespace Bunkering.Access.DAL
{
    public class AppointmentRepository : Repository<Appointment>, IAppointment
    {
        public AppointmentRepository(ApplicationContext context) : base(context)
        {
        }
    }

    public interface IAppointment : IRepository<Appointment> { }
}
