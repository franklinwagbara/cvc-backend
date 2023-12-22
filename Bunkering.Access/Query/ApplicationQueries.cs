using Bunkering.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Access.Query
{
    public class ApplicationQueries
    {
        private readonly ApplicationContext _applicationContext;
        public ApplicationQueries(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task<List<Application>> GetApplicationsByStateId(int stateId)
        {
            var apps = (from d in _applicationContext.Depots.AsQueryable()
                       join ad in _applicationContext.ApplicationDepots on d.Id equals ad.DepotId
                       join a in _applicationContext.Applications on ad.AppId equals a.Id
                       where d.StateId == stateId
                       select a).ToList();
            return apps;
        }
    }
}
