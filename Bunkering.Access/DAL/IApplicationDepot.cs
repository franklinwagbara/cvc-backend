using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.ViewModels;

namespace Bunkering.Access.DAL
{
    public interface IApplicationDepot : IRepository<ApplicationDepot>
    {
        public Task<IEnumerable<DepotViewModel>> GetDepotsAsync(int appId);
    }
}
