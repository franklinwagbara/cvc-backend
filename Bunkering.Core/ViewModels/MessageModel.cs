using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
    public class MessageModel
    {
       
        public int? ApplicationId { get; set; }
        public string Content { get; set; }
        public string Subject { get; set; }
        public string UserId { get; set; }
    }
}
