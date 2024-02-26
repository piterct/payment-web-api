using Microsoft.EntityFrameworkCore;
using Payment.Business.Interfaces.Repositories;
using Payment.Business.Models;
using Payment.Data.Contexts;

namespace Payment.Data.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(PaymentDbContext context) : base(context) { }

        public async Task<Order?> GetAllItemsById(Guid id)
        {
            return await Db.Orders.AsNoTracking()
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
    }
}
