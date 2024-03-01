using Payment.Business.Interfaces.Data;
using Payment.Business.Models;

namespace Payment.Business.Interfaces.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order?> GetAllItemsById(Guid id);
    }
}
