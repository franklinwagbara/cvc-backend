namespace Bunkering.Core.Data
{
    public class COQSubmittedDocument
    {
        public int Id { get; set; }
        public int CoQId { get; set; }
        public int FileId { get; set; }
        public int DocId { get; set; }
        public string DocSource { get; set; }
        public string DocType { get; set; }
        public string DocName { get; set; }
    }
}
