using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bunkering.Core.Data
{
    public class DemandNotice
    {
        public int Id { get; set; }
        public int DebitNoteId { get; set; }
        public string? Reference { get; set; }
        public double Amount { get; set; }
        public DateTime AddedDate { get; set; }
        public string AddedBy { get; set; }
        public string Description { get; set; }
        public bool Paid { get; set; }
        [ForeignKey(nameof(DebitNoteId))]
        public Payment DebitNote { get; set; }
    }
}