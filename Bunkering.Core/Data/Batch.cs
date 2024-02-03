using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class Batch
    {
        [Key]
        public int BatchId { get; set; }
        public string Name { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
