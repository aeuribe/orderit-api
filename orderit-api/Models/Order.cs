namespace orderit_api.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int PONumber { get; set; }
        public DateTime Date { get; set; }
        public int Status { get; set; }
        public int Total { get; set; }

        public Store Store { get; set; }
        public int StoreId { get; set; }

        public Salesperson Salesperson { get; set; }
        public int SalespersonId { get; set; }
        public ICollection<OrderDetail> Details { get; set; }
    }
}
