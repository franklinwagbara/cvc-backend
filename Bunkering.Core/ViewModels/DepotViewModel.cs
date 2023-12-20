namespace Bunkering.Core.ViewModels
{
    public class DepotViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public decimal Capacity { get; set; }
        public int ProductId { get; set; }
        public decimal Volume {  get; set; }
    }
}
