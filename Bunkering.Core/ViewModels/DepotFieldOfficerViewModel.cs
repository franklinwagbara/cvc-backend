﻿namespace Bunkering.Core.ViewModels
{
    public class DepotFieldOfficerViewModel
    {
        public int PlantFieldOfficerID { get; set; }
        public int DepotID { get; set; }
        public Guid UserID { get; set; }
        public string? DepotName { get; set;}
        public string? OfficerName { get; set;}
    }

}
