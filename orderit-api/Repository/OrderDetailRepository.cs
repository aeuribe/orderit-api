using Microsoft.EntityFrameworkCore;
using orderit_api.Data;
using orderit_api.Interfaces;
using orderit_api.Models;

namespace orderit_api.Repository
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        BusinessContext _context;

        public OrderDetailRepository(BusinessContext context)
        {
            _context = context;   
        }

        public OrderDetail AssignFKs(Product product, Order order, OrderDetail orderDetail)
        {
            orderDetail.Order = order;
            orderDetail.OrderId = order.OrderId;

            orderDetail.Product = product;
            orderDetail.ProductId = product.ProductId;

            return orderDetail;
        }

        public bool CreateOrderDetail(OrderDetail orderDetail)
        {
            _context.Add(orderDetail);
            return Save();
        }

        public bool DeleteOrderDetail(OrderDetail orderDetail)
        {
            _context.Remove(orderDetail);   
            return Save();
        }

        public bool DeleteOrderDetails(List<OrderDetail> details)
        {
            _context.RemoveRange(details);
            return Save();
        }

        public OrderDetail GetOrderDetailById(int id)
        {
            return _context.OrderDetails
                .Where(o => o.OrderDetailId == id)
                .Include(od => od.Product)
                .FirstOrDefault();
        }

        public OrderDetail GetOrderDetailById(int orderId, int orderDetailId)
        {
            return _context.OrderDetails
                .Where(od => od.OrderId == orderId && od.OrderDetailId == orderDetailId)
                .Include(od => od.Product)
                .FirstOrDefault();
        }

        public ICollection<OrderDetail> GetOrdersDetailByOrderId(int orderId)
        {
            return _context.OrderDetails
                .Where(o=>o.OrderId == orderId)
                .Include(od => od.Product)
                .ToList();
        }

        public bool OrderDetailExist(int orderDetailId)
        {
            return _context.OrderDetails.Any(o => o.OrderDetailId == orderDetailId);
        }

        public bool OrderDetailWithProductExist(int orderDetailId, int productId)
        {
            return _context.OrderDetails.Any(o => o.OrderDetailId == orderDetailId && o.ProductId == productId);
        }

        public bool OrderDetailExists(int orderId, int productId)
        {
            return _context.OrderDetails.Any(od => od.OrderId == orderId && od.ProductId == productId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;   
        }

        public bool UpdateOrderDetail(OrderDetail orderDetail)
        {
            _context.Update(orderDetail);
            return Save();
        }
    }
}
