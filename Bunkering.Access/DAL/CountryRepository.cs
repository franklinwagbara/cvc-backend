using Bunkering.Access.IContracts;
using Bunkering.Core.Data;

namespace Bunkering.Access.DAL
{
    public class CountryRepository : Repository<Country>, ICountry
    {
        public CountryRepository(ApplicationContext context) : base(context)
        {
        }
    }

    public interface ICountry : IRepository<Country> { }
}
