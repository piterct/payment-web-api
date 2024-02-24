using Payment.Business.Models;

namespace Payment.Business.Interfaces.Services
{
    public interface  ISellerService
    {
        Task<bool> Add(Seller seller);
    }
}
