namespace Bunkering.Core.ViewModels
{
    public class AppDepotViewModel
    {
        public int Id { get; set; }
        public int DepotId { get; set; }
        public int? AppId { get; set; }
        public string Name { get; set; }
        public int ProductId { get; set; }
        public int Volume {  get; set; }
    }
}