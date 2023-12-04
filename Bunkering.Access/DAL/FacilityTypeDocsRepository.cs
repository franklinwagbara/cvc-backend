using Bunkering.Access.IContracts;
using Bunkering.Core.Data;

namespace Bunkering.Access.DAL
{
    public class FacilityTypeDocsRepository : Repository<FacilityTypeDocument>, IFacilityTypeDocuments
    {
        public FacilityTypeDocsRepository(ApplicationContext context) : base(context)
        {
        }
    }

    public interface IFacilityTypeDocuments : IRepository<FacilityTypeDocument> { }
}