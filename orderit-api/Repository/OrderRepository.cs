using Microsoft.EntityFrameworkCore;
using orderit_api.Data;
using orderit_api.Interfaces;
using orderit_api.Models;

namespace orderit_api.Repository
{
    public class OrderRepository : IOrderRepository
    {
        BusinessContext _context;

        public OrderRepository(BusinessContext context)
        {
            _context = context;
        }

        public Order AssignFK(Salesperson salesperson, Store store, Order order)
        {
            order.Store = store;
            order.Salesperson = salesperson;
            order.StoreId = store.StoreId;
            order.SalespersonId = salesperson.SalespersonId;

            return order;
        }

        public bool CreateOrder(Order order)
        {
            _context.Add(order);
            return Save();

        }

        public Order GetOrderById(int id)
        {
            return _context.Orders
                .Where(o => o.OrderId == id)
                .Include(s => s.Salesperson)
                .Include(st => st.Store)
                .FirstOrDefault();
        }

        public Order GetOrderByPO(int PONumber)
        {
            return _context.Orders
                .Where(o => o.PONumber == PONumber)
                .Include(s => s.Salesperson)
                .Include(st => st.Store)
                .FirstOrDefault();
        }
        public bool UpdateOrder(Order order)
        {
            _context.Update(order);
            return Save();
        }

        public ICollection<Order> GetOrders()
        {
            return _context.Orders
                .Include(o => o.Details)
                .Include(s => s.Salesperson)
                .Include(st => st.Store)
                .ToList();
        }

        public bool OrderExist(int id)
        {
            return _context.Orders.Any(o => o.OrderId == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool DeleteOrder(Order order)
        {
            _context.Remove(order);
            return Save();
        }
    }
}
