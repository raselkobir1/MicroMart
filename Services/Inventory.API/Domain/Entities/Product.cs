namespace Inventory.API.Domain.Entities
{
    public class Product
    {
        public long Id { get; set; } 
        public string Name { get; set; } 
        public string SKU { get; set; }  
        public string Description { get; set; }   
    }
}
