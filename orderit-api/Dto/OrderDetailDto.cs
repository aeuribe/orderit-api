using orderit_api.Models;

namespace orderit_api.Dto
{
    public class OrderDetailDto
    {
        public int OrderDetailId { get; set; }
        public int quantity { get; set; }
        public ProductDto Product { get; set; }
    }
}
