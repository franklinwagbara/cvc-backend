using Bunkering.Access.IContracts;
using Bunkering.Core.Data;

namespace Bunkering.Access.DAL
{
    public class StateRepository : Repository<State>, IState
    {
        public StateRepository(ApplicationContext context) : base(context)
        {
        }
    }

    public interface IState : IRepository<State> { }
}
