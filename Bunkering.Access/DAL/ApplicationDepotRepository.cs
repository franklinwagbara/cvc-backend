using Bunkering.Core.Data;
using Bunkering.Core.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Bunkering.Access.DAL
{
    public class ApplicationDepotRepository : Repository<ApplicationDepot>, IApplicationDepot
    {
        private readonly ApplicationContext _context;
        public ApplicationDepotRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<DepotViewModel>> GetDepotsAsync(int appId)
        {
            return await _context.ApplicationDepots.Where(c => c.AppId == appId).Include(c => c.Depot).Select(x => new DepotViewModel
            {
                Id = x.DepotId,
                Name = x.Depot.Name,
                State = x.Depot.State,
            }).ToListAsync();
        }
    }
}
