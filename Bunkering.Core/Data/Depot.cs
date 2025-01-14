﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Bunkering.Core.Data
{
    public class Depot
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StateId { get; set; }
        public decimal Capacity { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
        public string? MarketerName { get; set; }
        //public int ProductId { get; set; }
        //public decimal Volume {  get; set; }

        [ForeignKey(nameof(StateId))]
        public State? State { get; set; }
    }
}
