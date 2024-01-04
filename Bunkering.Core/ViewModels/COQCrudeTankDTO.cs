using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
    public class COQCrudeTankDTO
    {
        [Required]
        public int CoQId { get; set; }
        public int MeasurementTypeId { get; set; }
        [Required]
        public string TankName { get; set; }
        [Required]
        public double DIP { get; set; }
        [Required]
        public double WaterDIP { get; set; }
        [Required]
        public double TOV { get; set; }
        [Required]
        public double WaterVolume { get; set; }
        [Required]
        public double FloatRoofCorr { get; set; }
        public double GOV { get; set; }
        [Required]
        public decimal Temp { get; set; }
        [Required]
        public double Density { get; set; }
        [Required]
        public double VCF { get; set; }
        private double GSV  => VCF * GOV;
        private double MTVAC => Density * GOV;
    }
}
