namespace Bunkering.Core.ViewModels
{
    public class CreateCoQViewModel
    {
        public int Id { get; set; }
        public int AppId { get; set; }
        public int DepotId { get; set; }
        public DateTime DateOfVesselArrival { get; set; }
        public DateTime DateOfVesselUllage { get; set; }
        public DateTime DateOfSTAfterDischarge { get; set; }
        public Decimal MT_VAC { get; set; }
        public Decimal MT_AIR { get; set; }
        public Decimal GOV { get; set; }
        public Decimal GSV { get; set; }
        public Decimal DepotPrice { get; set; }
    }

}
