using System.ComponentModel.DataAnnotations;

namespace orderit_api.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int quantity { get; set; }

        public Product Product { get; set; }
        public int ProductId { get; set; }

        public Order Order { get; set; }
        public int OrderId { get; set; }

    }
}
