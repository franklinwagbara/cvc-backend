using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Utils
{
    public class MailSettings
    {
        public string mailSender { get; set; }
        public string mailBCC { get; set; }
        public string mailHost { get; set; }
        public string mailPass { get; set; }
        public string UserName { get; set; }
        public bool UseSsl { get; set; }
        public bool WFile { get; set; }
        public int ServerPort { get; set; }
        public string npowr { get; set; }
        public string EQSBaseUrl { get; set; }
        public string EQSRelativeUrl { get; set; }
    }
}
