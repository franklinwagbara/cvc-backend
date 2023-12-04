using Bunkering.Access.IContracts;
using Bunkering.Core.Data;

namespace Bunkering.Access.DAL
{
	public class FacilityTypeRepository : Repository<FacilityType>, IFacilityType
	{
		public FacilityTypeRepository(ApplicationContext context) : base(context)
		{
		}
	}

	public interface IFacilityType : IRepository<FacilityType> { }
}
