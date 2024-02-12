using Bunkering.Access.IContracts;
using Bunkering.Core.Data;

namespace Bunkering.Access.DAL
{
    public class MeterTypeRepository : Repository<MeterType>, IMeterType
    {
        public MeterTypeRepository(ApplicationContext context) : base(context) { }
       
    }

    public interface IMeterType : IRepository<MeterType> { }
   
}
