using Payment.Business.Interfaces.Repositories;
using Payment.Business.Models;
using Payment.Data.Contexts;

namespace Payment.Data.Repositories
{
    public class SellerRepository : Repository<Seller>, ISellerRepository
    {
        public SellerRepository(PaymentDbContext context) : base(context) { }
    }
}
