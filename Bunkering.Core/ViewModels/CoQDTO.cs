using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
    public class CoQDTO
    {
        public int PlantId {  get; set; }
        public int NoaAppId {  get; set; }
        public double QuauntityReflectedOnBill {  get; set; }
        public double ArrivalShipFigure {  get; set; }
        public double DischargeShipFigure {  get; set; }
        public DateTime DateOfVesselArrival {  get; set; }
        public DateTime DateOfVesselUllage {  get; set; }
        public DateTime DateOfSTAfterDischarge {  get; set; }
        public double DepotPrice {  get; set; }
        public string NameConsignee {  get; set; }
       
    }
}
