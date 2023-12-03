using Bunkering.Access.IContracts;
using Bunkering.Core.Data;

namespace Bunkering.Access.DAL
{
    public class ValidatiionResponseRepo : Repository<ValidatiionResponse>, IValidatiionResponse
    {
        public ValidatiionResponseRepo(ApplicationContext context) : base(context)
        {
        }
    }

    public interface IValidatiionResponse : IRepository<ValidatiionResponse> { }
}
