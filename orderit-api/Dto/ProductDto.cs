using orderit_api.Models;

namespace orderit_api.Dto
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public string? Weight { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public BrandDto? Brand { get; set; }
        public CategoryDto? Category { get; set; }
    }
}
