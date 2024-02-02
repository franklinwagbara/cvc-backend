using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
    public class BatchViewModel
    {
        public int BatchId { get; set; }
        public string Name { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
