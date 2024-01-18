using Bunkering.Access.IContracts;
using Bunkering.Core.Data;

namespace Bunkering.Access.DAL
{
    public class PaymentRepository : Repository<Payment>, IPayment
    {
        public PaymentRepository(ApplicationContext context) : base(context){ }
    }

    public interface IPayment : IRepository<Payment> { }
}
