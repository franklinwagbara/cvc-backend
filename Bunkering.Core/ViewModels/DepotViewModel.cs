﻿namespace Bunkering.Core.ViewModels
{
    public class DepotViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public decimal Capacity { get; set; }
        public string ProductName { get; set; }
        //public int ProductId { get; set; }
        public decimal Volume {  get; set; }
    }
}
