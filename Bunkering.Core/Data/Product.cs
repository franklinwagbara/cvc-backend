namespace Bunkering.Core.Data
{
    public class Product
    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProductType { get; set; }
        public string? RevenueCode { get; set; }
        public string? RevenueCodeDescription { get; set; }
        public bool IsDeleted { get; set; }
    }   
}