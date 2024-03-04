using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
    public class SAPCreateDNoteResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
        public int sapDocEntry { get; set; }
        public int sapDocNum { get; set; }
        public DateTime docDate { get; set; }
        public DateTime docDueDate { get; set; }
        public string portalId { get; set; }
        public string? payment { get; set; }
    }
}
