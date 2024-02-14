using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.ViewModels
{
    public class DebitNoteSAPRequestDTO
    {
        public string Id { get; set; }
        public string Location { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string PostingDate { get; set; }
        public string DebitNoteType { get; set; }
        public string Directorate { get; set; }
        public string BankAccount { get; set; }
        public string DocumentCurrency { get; set; }
        public bool IsPaid { get; set; }
        public string PaymentReference { get; set; }
        public double PaymentAmount { get; set; }
        public List<DebitNoteLine> Lines { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhoneNumber1 { get; set; }
        public string CustomerPhoneNumber2 { get; set; }
        public string CustomerState { get; set; }
        public List<DebitNoteContact> Contacts { get; set; }
    }

    public class DebitNoteLine
    {
        public string RevenueCode { get; set; }
        public string RevenueDescription { get; set; }
        public int quantity => 1;
        public double ShoreVolume { get; set; }
        public double WholeSalePrice { get; set; }
        public double AppliedFactor { get; set; }
        public string Directorate { get; set; }
        public string ProductOrServiceType { get; set; }
        public string DaughterVesselName { get; set; }
        public string MotherVesselName { get; set; }
        public string Supplier { get; set; }
        public string Depot { get; set; }
    }

    public class DebitNoteContact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
