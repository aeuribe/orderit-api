using orderit_api.Models;

namespace orderit_api.Interfaces
{
    public interface IOrderRepository
    {
        Order GetOrderById(int id);
        Order GetOrderByPO(int PONumber);
        Order AssignFK(Salesperson salesperson, Store store, Order order);
        ICollection<Order> GetOrders();
        bool CreateOrder (Order order);
        bool UpdateOrder (Order order);
        bool DeleteOrder (Order order);
        bool OrderExist(int id);
        bool Save();
    }
}
