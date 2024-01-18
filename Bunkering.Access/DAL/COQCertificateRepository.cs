using Bunkering.Access.IContracts;
using Bunkering.Core.Data;

namespace Bunkering.Access.DAL
{
    public class COQCertificateRepository : Repository<COQCertificate>, ICOQCertificate
    {
        public COQCertificateRepository(ApplicationContext context) : base(context)
        {
        }
    }

    public interface ICOQCertificate: IRepository<COQCertificate>{}
}