using Bunkering.Access.IContracts;
using Bunkering.Core.Data;

namespace Bunkering.Access.DAL
{
    public class ApplicationSurveyorRepository : Repository<ApplicationSurveyor>, IApplicationSurveyor
    {
        public ApplicationSurveyorRepository(ApplicationContext context) : base(context)
        {
        }

    }
    public interface IApplicationSurveyor : IRepository<ApplicationSurveyor>
    {
    }
}
