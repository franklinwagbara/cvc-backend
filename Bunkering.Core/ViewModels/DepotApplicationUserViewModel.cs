using Bunkering.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
    public class DepotApplicationUserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public decimal Capacity { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }

        public IEnumerable<Application>? Applications { get; set; }
       
    }
}
