using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
    public class DebitNoteSAPRequestDTO
    {
        public string id { get; set; }
        public string location { get; set; }
        public string customerCode { get; set; }
        public string customerName { get; set; }
        public string postingDate { get; set; }
        public string debitNoteType { get; set; }
        public string directorate { get; set; }
        public string bankAccount { get; set; }
        public string documentCurrency { get; set; }
        public bool isPaid { get; set; }
        public string paymentReference { get; set; }
        public double paymentAmount { get; set; }
        public List<DebitNoteLine> lines { get; set; }
        public string customerAddress { get; set; }
        public string customerEmail { get; set; }
        public string customerPhoneNumber1 { get; set; }
        public string customerPhoneNumber2 { get; set; }
        public string customerState { get; set; }
        public List<DebitNoteContact> contacts { get; set; }
    }

    public class DebitNoteLine
    {
        public string revenueCode { get; set; }
        public string revenueDescription { get; set; }
        public int quantity => 1;
        public double shoreVolume { get; set; }
        public double wholeSalePrice { get; set; }
        public double appliedFactor { get; set; }
        public string directorate { get; set; }
        public string productOrServiceType { get; set; }
        public string daughterVesselName { get; set; }
        public string motherVesselName { get; set; }
        public string dupplier { get; set; }
        public string depot { get; set; }
    }

    public class DebitNoteContact
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
    }
}
