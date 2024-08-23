using orderit_api.Models;

namespace orderit_api.Dto
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int PONumber { get; set; }
        public DateTime Date { get; set; }
        public int Status { get; set; }
        public int Total { get; set; }
        public SalespersonDto Salesperson { get; set; }
        public StoreDto Store { get; set; }
    }
}
