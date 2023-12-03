using Bunkering.Access.IContracts;
using Bunkering.Core.Data;

namespace Bunkering.Access.DAL
{
    public class ProductRepository : Repository<Product>, IProduct
    {
        public ProductRepository(ApplicationContext context) : base(context)
        {
        }
    }

    public interface IProduct : IRepository<Product> { }
}
