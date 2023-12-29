namespace Bunkering.Core.ViewModels
{
    public class DebitNoteDTO
    {
        public DateTime VesselDischargeDate { get; }
        public DateTime PayableBefore { get; }
        public string Marketer { get; }
        public string DepotName { get; }
        public decimal Price { get; }
        public decimal Levy { get; }
        public decimal Volume { get; }
        public decimal WholesalePrice { get; }

        public DebitNoteDTO(DateTime vesselDischargeDate, DateTime payableBefore, string marketer, string depotName, decimal price, decimal levy, decimal volume, decimal wholesalePrice)
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
