using Bunkering.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Access.Query
{
    public class PlantQueries
    {
        private readonly ApplicationContext _context;
        public PlantQueries(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<List<Plant>> GetPlantsByCompanywithTanks(string email)
        {
            var plants = (from p in _context.Plants.AsQueryable()
                        join pt in _context.PlantTanks on p.Id equals pt.PlantId
                        where p.Email == email
                        select p).ToList();
            return plants;
        }
    }
}
