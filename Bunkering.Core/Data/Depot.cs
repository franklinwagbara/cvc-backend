namespace Bunkering.Core.Data
{
    public class Depot
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public decimal Capacity { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
        //public int ProductId { get; set; }
        //public decimal Volume {  get; set; }
       
    }
}
