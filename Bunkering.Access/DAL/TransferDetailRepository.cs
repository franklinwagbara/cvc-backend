using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Access.DAL
{
    public class TransferDetailRepository : Repository<TransferDetail>, ITransferDetail
    {
        public TransferDetailRepository(ApplicationContext context) : base(context)
        {
        }
    }

    public interface ITransferDetail : IRepository<TransferDetail> { }
}
