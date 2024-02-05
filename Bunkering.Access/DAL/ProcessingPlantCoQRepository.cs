using Bunkering.Access.IContracts;
using Bunkering.Core.Data;

namespace Bunkering.Access.DAL
{
    public class ProcessingPlantCoQRepository : Repository<ProcessingPlantCOQ>, IProcessingPlantCoQ
    {
        public ProcessingPlantCoQRepository(ApplicationContext context) : base(context)
        {
        }
    }

    public interface IProcessingPlantCoQ: IRepository<ProcessingPlantCOQ> { }
}
