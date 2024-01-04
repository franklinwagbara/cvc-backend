using System;
using System.Collections.Generic;
using System.Text;

namespace Bunkering.Core.ViewModels
{   

    public class SubmitDocumentDto
    {
        public int FileId { get; set; }
        public int DocId { get; set; }
        public string DocSource { get; set; }
        public string DocType { get; set; }
        public string DocName { get; set; }
    }

}
