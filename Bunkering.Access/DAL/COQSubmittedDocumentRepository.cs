using Bunkering.Access.IContracts;
using Bunkering.Core.Data;

namespace Bunkering.Access.DAL
{
    public class COQSubmittedDocumentRepository : Repository<COQSubmittedDocument>, ICOQSubmittedDocument
    {
        public COQSubmittedDocumentRepository(ApplicationContext context) : base(context)
        {
        }
    }

    public interface ICOQSubmittedDocument: IRepository<COQSubmittedDocument> { }
}
