namespace Bunkering.Core.ViewModels
{
    public class DebitNoteDTO
    {
        public DateTime VesselDischargeDate { get; }
        public DateTime PayableBefore { get; }
        public string Marketer { get; }
        public string DepotName { get; }
        public double Price { get; }
        public double Levy { get; }
        public double Volume { get; }
        public double WholesalePrice { get; }
        public string Location { get; set; }
        public string DebitNoteNumber { get; set; }
        public string MotherVessel { get; set; }
        public string DaughterVessel { get; set; }
        public string Supplier { get; set; }
        public string Product { get; set; }

        public DebitNoteDTO(DateTime vesselDischargeDate, DateTime payableBefore, string marketer, string depotName, double price, double levy, double volume, double wholesalePrice, string location, string debitNoteNumber, string motherVessel, string daugtherVessel, string supplier, string product)
        {
            VesselDischargeDate = vesselDischargeDate;
            PayableBefore = payableBefore;
            Marketer = marketer;
            DepotName = depotName;
            Price = price;
            Levy = levy;
            Volume = volume;
            WholesalePrice = wholesalePrice;
            Location = location;
            DebitNoteNumber = debitNoteNumber;
            MotherVessel = motherVessel;
            DaughterVessel = daugtherVessel;
            Supplier = supplier;
            Product = product;
        }
    }

}
