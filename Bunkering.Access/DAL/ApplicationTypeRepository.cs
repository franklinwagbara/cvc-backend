using Bunkering.Access.IContracts;
using Bunkering.Core.Data;

namespace Bunkering.Access.DAL
{
    public class ApplicationTypeRepository : Repository<ApplicationType>, IApplicationType
    {
        public ApplicationTypeRepository(ApplicationContext context) : base(context)
        {
        }
    }

    public interface IApplicationType : IRepository<ApplicationType> { }
}
