using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using orderit_api.Dto;
using orderit_api.Interfaces;
using orderit_api.Models;
using orderit_api.Repository;

namespace orderit_api.Controller
{
    [Route("api/orders/")]
    [ApiController]
    [Authorize]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public OrderDetailController(IOrderDetailRepository orderDetailRepository, IOrderRepository orderRepository,
            IMapper mapper, IProductRepository productRepository)
        {
            _orderDetailRepository = orderDetailRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        [HttpGet("{orderId}/details/{orderDetailId}")]
        [ProducesResponseType(200, Type = typeof(OrderDetail))]
        public IActionResult GetOrderDetailById(int orderId, int orderDetailId)
        {
            var orderdetail = _mapper.Map<OrderDetailDto>(_orderDetailRepository.GetOrderDetailById(orderId, orderDetailId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(orderdetail);
        }

        [HttpGet("{orderId}/details")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<OrderDetail>))]
        public IActionResult GetOrdersDetailByOrderId(int orderId)
        {
            if (!_orderRepository.OrderExist(orderId))
                return NotFound();

            var orderdetail = _mapper.Map<List<OrderDetailDto>>(_orderDetailRepository.GetOrdersDetailByOrderId(orderId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(orderdetail);
        }

        [HttpPost("{orderId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult CreateOrderDetail(int orderId, [FromQuery] int productId,
            [FromBody] OrderDetailDto orderDetailDto)
        {
            if (orderDetailDto == null)
                return BadRequest(ModelState);

            if (_orderDetailRepository.OrderDetailExists(orderId,productId))
            {
                ModelState.AddModelError("", "This product is already added to the order.");
                return StatusCode(404, ModelState);
            }

            if (!_orderRepository.OrderExist(orderId))
            {
                ModelState.AddModelError("", "Order does not exist");
                return StatusCode(404, ModelState);
            }

            if (!_productRepository.ProductExist(productId))
            {
                ModelState.AddModelError("", "Product does not exist");
                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Crear un nuevo OrderDetail usando el OrderId y los datos del DTO
            var orderDetailMap = _mapper.Map<OrderDetail>(orderDetailDto);
            var order = _orderRepository.GetOrderById(orderId);
            var product = _productRepository.GetProductById(productId);

            orderDetailMap = _orderDetailRepository.AssignFKs(product, order, orderDetailMap);

            // Intentar guardar el OrderDetail en la base de datos
            if (!_orderDetailRepository.CreateOrderDetail(orderDetailMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Order detail successfully created");
        }

        [HttpPut("{orderId}/details/{orderDetailId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateOrderDetail(int orderDetailId, int orderId, int productId,[FromBody] OrderDetailDto orderDetailDto) 
        {
            if (!_orderDetailRepository.OrderDetailExist(orderDetailId)) 
            { 
                return NotFound($"Detail {orderDetailId} not found");
            }

            if (!_orderRepository.OrderExist(orderId)) 
            {
                return NotFound($"Order {orderId} not found");
            }

            if (!_productRepository.ProductExist(productId))
            {
                return NotFound($"Product {productId} not found");
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (orderDetailId != orderDetailDto.OrderDetailId) return BadRequest(ModelState);

            if (orderDetailDto == null)
            {
                return BadRequest();
            }

            var order = _orderRepository.GetOrderById(orderId);
            var product = _productRepository.GetProductById(productId);

            var orderDetailMap = _mapper.Map<OrderDetail>(orderDetailDto);

            orderDetailMap = _orderDetailRepository.AssignFKs(product, order, orderDetailMap);

            if (!_orderDetailRepository.UpdateOrderDetail(orderDetailMap)) 
            {
                ModelState.AddModelError("", "Something went wrong updating order detail");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{orderId}/{orderDetailId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteOrderDetail( int orderId, int orderDetailId) 
        {
            if (!_orderDetailRepository.OrderDetailExist(orderDetailId)) 
            {
                return NotFound("Order detail not found");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var orderDetailToDelete = _orderDetailRepository.GetOrderDetailById(orderId,orderDetailId);

            if (!_orderDetailRepository.DeleteOrderDetail(orderDetailToDelete)) 
            {
                ModelState.AddModelError("", "Something went wrong deleting order detail");
            }

            return NoContent();

        }

        [HttpDelete("DeleteDetailsByOrder/{orderId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteDetailsByOrder(int orderId) 
        {
            if (!_orderRepository.OrderExist(orderId))
                return NotFound();

            var detailsToDelete = _orderDetailRepository.GetOrdersDetailByOrderId(orderId);

            if (!ModelState.IsValid)
                return BadRequest();
              
            if (!_orderDetailRepository.DeleteOrderDetails(detailsToDelete.ToList()))
            {
                ModelState.AddModelError("", "Something went wrong deleting details");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }

    }
}
