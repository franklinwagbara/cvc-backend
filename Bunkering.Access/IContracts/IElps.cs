using Bunkering.Core.Data;
using Bunkering.Core.Utils;

namespace Bunkering.Access.IContracts
{
    public interface IElps
    {
        Dictionary<string, string> GetCompanyDetailByEmail(string email);
        List<RegisteredAddress> GetCompanyRegAddress(int id);
        bool UpdateCompanyRegAddress(List<RegisteredAddress> model);
        object AddCompanyRegAddress(List<RegisteredAddress> model, int companyId);
        RegisteredAddress GetCompanyRegAddressById(int id);
        List<DirectorModel> GetCompanyDirectors(int id);
        object UpdateCompanyDetails(CompanyModel model, string email, bool update);
        string UpdateCompanyNameEmail(object model);
        List<DocumentType> GetDocumentTypes(string type = null);
        object GetCompanyDocuments(int id, string type = null);
        object GetFacilityDocuments(int id);
        //List<MailTemplate> GetMailMessages();
        int PushPermitToElps(PermitAPIModel item);
        string CreateElpsFacility(object item);
        string UpdateElpsFacility(object item);
        Dictionary<string, object> ConfirmRemitaStatus(int id);
        Dictionary<string, object> ConfirmEtxraRemitaStatus(int id);
        Dictionary<string, object> ByPassPayment(int id);
        //string PostReferenceToIGR(string baseUrl, string reqeustUri, object payload);
        Task<RemitaResponse> GeneratePaymentReference(string baseUrl, Application application, decimal totalAmount, decimal serviceCharge);
        Task<RemitaResponse> GenerateDebitNotePaymentReference(string baseUrl, double totalAmount, string companyName, string companyEmail, string appRef, string depotName, int compElpsId, string paymentType, string paymentDescription);
        Task<Payment> GenerateExtraPaymentReference(string baseUrl, Application application, Payment payment, decimal totalAmount, decimal serviceCharge);
        List<Staff> GetAllStaff();
        Staff GetStaff(string email);
        Dictionary<string, string> ChangePassword(object model, string useremail);
        //MistdoResponse VerifyMISTDOCert(string cert);
    }
}
