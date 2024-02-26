using Payment.Business.Interfaces.Notifications;
using Payment.Business.Interfaces.Repositories;
using Payment.Business.Interfaces.Services;
using Payment.Business.Models;
using Payment.Business.Validations;

namespace Payment.Business.Services
{
    public class SellerService : BaseService, ISellerService
    {
        private readonly ISellerRepository _sellerRepository;
        public SellerService(INotifier notifier, ISellerRepository sellerRepository) : base(notifier)
        {
            _sellerRepository = sellerRepository;
        }
        public async Task<bool> Add(Seller seller)
        {
            if (!RunValidation(new SellerValidation(), seller)) return false;

            var newSeller = await _sellerRepository.GetById(seller.Id);

            if (newSeller is not null)
            {
                Notify("Seller with this ID already added.");
                return false;
            }

            await _sellerRepository.Add(seller);

            return true;
        }
    }
}
