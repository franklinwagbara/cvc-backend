using System.ComponentModel.DataAnnotations.Schema;

namespace Bunkering.Core.Data
{
    public class COQCertificate
    {
        public int Id { get; set; }
        public int? COQId { get; set; }
        public int? ProductId { get; set; }
        public int ElpsId { get; set; }
        public DateTime ExpireDate { get; set; }
        public DateTime IssuedDate { get; set; }
        public string CertifcateNo { get; set; } = string.Empty;
        public string Signature { get; set; } = string.Empty;
        public string QRCode { get; set; } = string.Empty;
        [ForeignKey(nameof(COQId))]
        public CoQ? COQ { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }
    }
}