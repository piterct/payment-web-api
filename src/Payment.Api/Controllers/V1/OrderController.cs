using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Payment.Api.ViewModels;
using Payment.Business.Dtos;
using Payment.Business.Enums;
using Payment.Business.Interfaces.Notifications;
using Payment.Business.Interfaces.Queries;
using Payment.Business.Interfaces.Repositories;
using Payment.Business.Interfaces.Services;
using Payment.Business.Models;

namespace Payment.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class OrderController : MainController
    {
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;
        private readonly IOrderQuery _orderQuery;
        private readonly ISellerRepository _sellerRepository;
        private readonly IOrderRepository _orderRepository;
        public OrderController(INotifier notifier, IMapper mapper,
            IOrderService orderService,
            IOrderQuery orderQuery,
            ISellerRepository sellerRepository,
            IOrderRepository orderRepository) : base(notifier)
        {
            _mapper = mapper;
            _orderService = orderService;
            _orderQuery = orderQuery;
            _sellerRepository = sellerRepository;
            _orderRepository = orderRepository;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            var order = await _orderQuery.GetById(id);

            if (order.OrderId == Guid.Empty)
            {
                NotifyError("Order not found");
                return CustomResponseNotFound();
            }

            return CustomResponse(order);
        }

        [HttpPost]
        public async Task<ActionResult> AddOrder(OrderRequest orderRequest)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var seller = await _sellerRepository.GetById(Guid.Parse(orderRequest.SellerId));

            if (seller == null)
            {
                NotifyError("Seller not found");
                return CustomResponseNotFound();
            }

            var newOrder = await _orderService.Add(_mapper.Map<OrderDto>(orderRequest));
            return CustomResponse(newOrder);
        }

        [HttpPut("{id:guid}/status/{orderStatus}")]
        public async Task<ActionResult> UpdateOrderStatus(Guid id, EOrderStatus orderStatus)
        {
            var order = await _orderRepository.GetAllItemsById(id);

            if (order == null)
            {
                NotifyError("Order not found");
                return CustomResponseNotFound();
            }

            var statusOrderUpdated = await _orderService.UpdateOrderStatus(order, orderStatus);

            return CustomResponse(statusOrderUpdated);
        }
    }
}
