using Bunkering.Access.IContracts;
using Bunkering.Core.Data;

namespace Bunkering.Access.DAL
{
    public class PPCOQSubmittedDocumentRepository : Repository<PPCOQSubmittedDocument>, IPPCOQSubmittedDocument
    {
        public PPCOQSubmittedDocumentRepository(ApplicationContext context) : base(context)
        {
        }
    }

    public interface IPPCOQSubmittedDocument : IRepository<PPCOQSubmittedDocument> { }
}
