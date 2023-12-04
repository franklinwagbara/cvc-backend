using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class LGA
    {
        public int Id { get; set; }
        public int StateId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        [ForeignKey(nameof(StateId))]
        public State State { get; set; }
    }
}
