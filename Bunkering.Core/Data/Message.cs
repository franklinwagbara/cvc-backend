using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class Message
    {
        public int Id { get; set; }
        public bool IsRead { get; set; }
        public int? ApplicationId { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public string Subject { get; set; }
        public string UserId { get; set; }
    }
}
