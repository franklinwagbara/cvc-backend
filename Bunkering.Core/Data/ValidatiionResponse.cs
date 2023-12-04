using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class ValidatiionResponse
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string LicenseNo { get; set; }
        public string Response { get; set; }
        public bool IsUsed { get; set; }
    }
}
