using Payment.Business.Dtos;
using Payment.Business.Enums;
using Payment.Business.Models;

namespace Payment.Business.Interfaces.Services
{
    public interface IOrderService
    {
        Task<NewOrderDto> Add(Order order);
        Task<OrderDto> UpdateOrderStatus(Order order, EOrderStatus newStatus);
    }
}
