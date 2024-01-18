using Bunkering.Access.IContracts;
using Bunkering.Core.Data;

namespace Bunkering.Access.DAL
{
    public class MessageRepository : Repository<Message>, IMessage
    {
        public MessageRepository(ApplicationContext context) : base(context)
        {
        }
    }

    public interface IMessage : IRepository<Message> { }
}
