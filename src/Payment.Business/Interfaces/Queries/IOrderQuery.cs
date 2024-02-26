using Payment.Business.Dtos;

namespace Payment.Business.Interfaces.Queries
{
    public interface  IOrderQuery
    {
        Task<OrderDto> GetById(Guid orderId);
    }
}
