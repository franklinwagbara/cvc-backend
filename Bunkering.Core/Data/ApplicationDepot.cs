using System.ComponentModel.DataAnnotations.Schema;

namespace Bunkering.Core.Data
{
    public class ApplicationDepot
    {
        public int Id { get; set; }
        public int DepotId { get; set; }
        [ForeignKey(nameof(DepotId))]
        public Plant Depot { get; set; }
        public int AppId { get; set; }
        [ForeignKey(nameof(AppId))]
        public Application Application { get; set; }
        public decimal Volume { get; set; }
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
        public string? DischargeId { get; set; } = string.Empty;

    }
}
