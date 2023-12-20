using Bunkering.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Access.Query
{
    public class UserQueries
    {
        private readonly ApplicationContext _applicationContext;
        public UserQueries(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        //public async Task<int> GetUserStateByOfficeID(int officeId)
        //{
        //    var stateId = from d in _applicationContext.ApplicationsDepot.AsQueryable()
        //                    join a in _applicationContext.Applications on d.a
        //}
    }
}
