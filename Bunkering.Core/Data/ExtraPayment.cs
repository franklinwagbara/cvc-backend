using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bunkering.Core.Data
{
    public class ExtraPayment
    {
        public int Id { get; set; }
        public string Reference { get; set; }
        public DateTime AddedDate { get; set; }
        public string AddedBy { get; set; }
    }
}