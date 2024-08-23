using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using orderit_api.Dto;
using orderit_api.Interfaces;
using orderit_api.Models;
using orderit_api.Repository;

namespace orderit_api.Controller
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly ISalespersonRepository _salespersonRepository;
        private readonly IMapper _mapper;

        public OrderController(IOrderRepository orderRepository,
            IStoreRepository storeRepository, ISalespersonRepository salespersonRespository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _storeRepository = storeRepository;
            _salespersonRepository = salespersonRespository;
            _mapper = mapper;
        }

        [HttpGet]/**/
        [ProducesResponseType(200, Type = typeof(IEnumerable<Order>))]
        public IActionResult GetOrders()
        {
            var orders = _mapper.Map<List<OrderDto>>(_orderRepository.GetOrders());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        [ProducesResponseType(200, Type = typeof(Order))]
        public IActionResult GetOrder(int orderId)
        {
            if (!_orderRepository.OrderExist(orderId))
                return NotFound();

            var orders = _mapper.Map<OrderDto>(_orderRepository.GetOrderById(orderId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(orders);
        }

        [HttpGet("PO")]
        [ProducesResponseType(200, Type = typeof(Order))]
        public IActionResult GetOrderByPO([FromQuery] int PONumber)
        {

            var orders = _mapper.Map<OrderDto>(_orderRepository.GetOrderByPO(PONumber));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(orders);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateOrder([FromQuery] int storeId, [FromQuery] int salespersonId,
            [FromBody] OrderDto orderCreate) 
        {
            if (orderCreate == null)
                return BadRequest(ModelState);

            var orders = _orderRepository.GetOrders()
                 .Where(s => s.PONumber == orderCreate.PONumber)
                 .FirstOrDefault();

            if (orders != null)
            {
                ModelState.AddModelError("", "Order already exists");
                return StatusCode(422, ModelState);
            }

            if (!_storeRepository.StoreExist(storeId))
            {
                NotFound();
            }

            if (!_salespersonRepository.SalespersonExist(salespersonId))
            {
                NotFound();
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var store = _storeRepository.GetById(storeId);

            var salesperson = _salespersonRepository.GetById(salespersonId);

            var order = new Order
            {
                PONumber = orderCreate.PONumber,
                Date = DateTime.UtcNow, // Fecha y hora actual
                Status = 1, // Estado predeterminado, por ejemplo "Pendiente"
            };

            var orderMap = _mapper.Map<Order>(order);

            orderMap = _orderRepository.AssignFK(salesperson, store, order);

            if (!_orderRepository.CreateOrder(orderMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok(new { OrderId = orderMap.OrderId });

        }

        [HttpPut("{orderId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateOrder( int orderId, int storeId, int salespersonId, [FromBody] OrderDto orderDto)
        {
            if (!_orderRepository.OrderExist(orderId))
            {
                return NotFound();
            }

            if (!_storeRepository.StoreExist(storeId)) 
            {
                return NotFound();
            }

            if (!_salespersonRepository.SalespersonExist(salespersonId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (orderId != orderDto.OrderId) return BadRequest(ModelState);

            if (orderDto == null)
            {
                return BadRequest();
            }

            var store = _storeRepository.GetById(storeId);
            var salesperson = _salespersonRepository.GetById(salespersonId);
            var orderMap = _mapper.Map<Order>(orderDto);

            orderMap = _orderRepository.AssignFK(salesperson, store, orderMap);

            if (!_orderRepository.UpdateOrder(orderMap))
            {
                ModelState.AddModelError("", "Something went wrong updating order");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{orderId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteOrder(int orderId)
        {
            if (!_orderRepository.OrderExist(orderId))
            {
                return NotFound("Order not found");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var orderToDelete = _orderRepository.GetOrderById(orderId);

            if (!_orderRepository.DeleteOrder(orderToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting order");
            }

            return NoContent();

        }
    }


}
