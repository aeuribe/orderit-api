namespace orderit_api.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public string? Weight {  get; set; }
        public string? ImageUrl { get; set;}
        public bool IsActive { get; set; }

        public Brand Brand { get; set; }
        public int BrandId { get; set; }

        public Category Category { get; set; }
        public int CategoryId { get; set; }
    }
}
