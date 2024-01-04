namespace Bunkering.Core.Data
{
    public class Message
    {
        public int Id { get; set; }
        public bool IsRead { get; set; }
        public int? ApplicationId { get; set; }
        public int? COQId { get; set; }
        public bool? IsCOQ { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public string Subject { get; set; }
        public string UserId { get; set; }
    }
}
