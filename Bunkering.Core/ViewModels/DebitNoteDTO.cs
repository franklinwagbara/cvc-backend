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

        public DebitNoteDTO(DateTime vesselDischargeDate, DateTime payableBefore, string marketer, string depotName, double price, double levy, double volume, double wholesalePrice)
        {
            VesselDischargeDate = vesselDischargeDate;
            PayableBefore = payableBefore;
            Marketer = marketer;
            DepotName = depotName;
            Price = price;
            Levy = levy;
            Volume = volume;
            WholesalePrice = wholesalePrice;
        }
    }

}
