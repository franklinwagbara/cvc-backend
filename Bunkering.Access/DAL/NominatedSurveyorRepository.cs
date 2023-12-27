using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Access.DAL
{
    public class NominatedSurveyorRepository : Repository<NominatedSurveyor>, INominatedSurveyor
    {
        private readonly ApplicationContext _context;
        public NominatedSurveyorRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public async Task<NominatedSurveyor?> GetNextAsync()
        {
            return await _context.NominatedSurveyors.OrderBy(c => c.NominatedVolume).FirstOrDefaultAsync();
        }
    }

    public interface INominatedSurveyor : IRepository<NominatedSurveyor>
    {
        public Task<NominatedSurveyor?> GetNextAsync();
    }
}
