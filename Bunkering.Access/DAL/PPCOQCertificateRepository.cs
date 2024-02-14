using Bunkering.Access.IContracts;
using Bunkering.Core.Data;

namespace Bunkering.Access.DAL
{
    public class PPCOQCertificateRepository : Repository<PPCOQCertificate>, IPPCOQCertificate
    {
        public PPCOQCertificateRepository(ApplicationContext context) : base(context)
        {
        }
    }

    public interface IPPCOQCertificate: IRepository<PPCOQCertificate> { }
}
