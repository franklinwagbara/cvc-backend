using System;
using System.Collections.Generic;
using System.Text;

namespace Bunkering.Core.ViewModels
{
    public class DocumentModel
    {
    }

    public class Document
    {
        public int id { get; set; }
        public int companyId { get; set; }
        public string fileName { get; set; }
        public string source { get; set; }
        public string document_type_id { get; set; }
        public string documentTypeName { get; set; }
        public string date_modified { get; set; }
        public string date_added { get; set; }
        public string status { get; set; }
        public string archived { get; set; }
        public string document_Name { get; set; }
        public string uniqueId { get; set; }
    }

    public class FacilityDocument
    {
        public int Company_Id { get; set; }
        public int Document_Type_Id { get; set; }
        public string documentTypeName { get; set; }
        public int FacilityId { get; set; }
        public string Name { get; set; }
        public string Source { get; set; }
        public DateTime Date_Modified { get; set; }
        public DateTime Date_Added { get; set; }
        public bool Status { get; set; }
        public bool Archived { get; set; }
        public string UniqueId { get; set; }
        public string Document_Name { get; set; }
        public DateTime Expiry_Date { get; set; }
        public int Id { get; set; }
        public string Type { get; set; }
    }
}
