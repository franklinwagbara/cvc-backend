using Bunkering.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Bunkering.Core.Data
{
	public class Facility
	{
		public int Id { get; set; }
		public int CompanyId { get; set; }
		public int ElpsId { get; set; }
		public int VesselTypeId { get; set; }
		public string Name { get; set; }
        public string IMONumber { get; set; }
        public string CallSIgn { get; set; }
        public string Flag { get; set; }
        public int YearOfBuild { get; set; }
        public string PlaceOfBuild { get; set; }
        public bool IsLicensed { get; set; }
		[ForeignKey(nameof(CompanyId))]
		public Company Company { get; set; }
		[ForeignKey(nameof(VesselTypeId))]
		public VesselType VesselType { get; set; }
		public decimal DeadWeight { get; set; }
		public decimal Capacity { get; set; }
		public string Operator { get; set; }
		public virtual ICollection<FacilitySource> FacilitySources { get; set; }
		public ICollection<Tank> Tanks { get; set; }

	}



}
