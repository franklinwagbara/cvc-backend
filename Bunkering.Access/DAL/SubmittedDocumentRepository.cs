using Bunkering.Access.IContracts;
using Bunkering.Core.Data;

namespace Bunkering.Access.DAL
{
    public class SubmittedDocumentRepository : Repository<SubmittedDocument>, ISubmittedDocument
    {
        public SubmittedDocumentRepository(ApplicationContext context) : base(context)
        {
        }
    }

    public interface ISubmittedDocument : IRepository<SubmittedDocument> { }
}
