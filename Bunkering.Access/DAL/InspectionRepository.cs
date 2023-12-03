using Bunkering.Access.IContracts;
using Bunkering.Core.Data;

namespace Bunkering.Access.DAL
{
    public class InspectionRepository : Repository<Inspection>, IInspection
    {
        public InspectionRepository(ApplicationContext context) : base(context)
        {
        }
    }

    public interface IInspection : IRepository<Inspection> { }
}
