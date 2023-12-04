using Bunkering.Access.IContracts;
using Bunkering.Core.Data;

namespace Bunkering.Access.DAL
{
    public class WorkflowRepository : Repository<WorkFlow>, IWorkflow
    {
        public WorkflowRepository(ApplicationContext context) : base(context)
        {
        }
    }

    public interface IWorkflow : IRepository<WorkFlow> { }
}
