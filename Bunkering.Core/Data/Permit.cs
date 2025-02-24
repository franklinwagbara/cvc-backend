﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Bunkering.Core.Data
{
    public class Permit
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public int ElpsId { get; set; }
        public DateTime ExpireDate { get; set; }
        public DateTime IssuedDate { get; set; }
        public string PermitNo { get; set; }
        public string Signature { get; set; }
        public string QRCode { get; set; }
        [ForeignKey(nameof(ApplicationId))]
        public Application Application { get; set; }
    }
}
