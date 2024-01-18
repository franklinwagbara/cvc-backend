using Bunkering.Access.IContracts;
using Bunkering.Core.Data;

namespace Bunkering.Access.DAL
{
	public class ApplicationRepository : Repository<Application>, IApplication
	{
		public ApplicationRepository(ApplicationContext context) : base(context)
		{

		}
	}

	public interface IApplication : IRepository<Application>
	{

	}
}
