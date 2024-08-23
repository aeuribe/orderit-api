using orderit_api.Models;

namespace orderit_api.Interfaces
{
    public interface IOrderDetailRepository
    {
        OrderDetail GetOrderDetailById(int orderId, int orderDetailId);
        OrderDetail AssignFKs(Product product, Order order, OrderDetail orderDetail);
        ICollection<OrderDetail> GetOrdersDetailByOrderId(int orderId);
        bool CreateOrderDetail(OrderDetail orderDetail);
        bool UpdateOrderDetail(OrderDetail orderDetail);
        bool DeleteOrderDetail(OrderDetail orderDetail);
        bool DeleteOrderDetails(List<OrderDetail> details);
        bool OrderDetailExist(int orderDetailId);
        bool OrderDetailExists(int orderId, int productId);
        bool Save();

    }
}
