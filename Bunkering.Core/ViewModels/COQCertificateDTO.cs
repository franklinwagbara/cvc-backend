using Bunkering.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
    public class COQCertificateDTO
    {
        public string CompanyName { get; set; }
        public string MotherVessel { get; set; }
        public DateTime DateOfVesselArrival { get; set; }
        public string Product { get; set; }
        public string Jetty { get; set; }
        public DateTime DateOfVessselUllage { get; set; }
        public string VesselName { get; set; }
        public string ReceivingTerminal { get; set; }
        public List<TankMeasurement> BeforeTankMeasurements { get; set; }
        public List<TankMeasurement> AfterTankMeasurement { get; set; }
        public List<TankMeasurement> Difference { get; set; }
    }
}
