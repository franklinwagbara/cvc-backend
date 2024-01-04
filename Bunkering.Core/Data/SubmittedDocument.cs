using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Core.Data
{
    public class SubmittedDocument
    {
        public int Id { get; set; }
        /// <summary>
        /// ApplicationId for NOA and CoQId for Coq
        /// </summary>
        public int ApplicationId { get; set; }
        public int ApplicationTypeId { get; set; }
        public int FileId { get; set; }
        public int DocId { get; set; }
        public string DocSource { get; set; }
        public string DocType { get; set; }
        public string DocName { get; set; }
        //public bool IsFAD { get; set; }
    }
}
