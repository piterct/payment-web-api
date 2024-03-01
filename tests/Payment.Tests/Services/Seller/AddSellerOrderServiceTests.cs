using Moq;
using Moq.AutoMock;
using Payment.Business.Interfaces.Repositories;
using Payment.Business.Services;

namespace Payment.Tests.Services.Seller
{
    public class AddSellerOrderServiceTests
    {
        private readonly AutoMocker _mocker;
        private readonly SellerService _sellerService;

        public AddSellerOrderServiceTests()
        {
            _mocker = new AutoMocker();
            _sellerService = _mocker.CreateInstance<SellerService>();
        }

        [Fact(DisplayName = "Add new valid seller")]
        [Trait("Category", "Seller Services")]
        public async void Seller_NewSeller_MustReturnTrue()
        {
            // Arrange
            _mocker.GetMock<ISellerRepository>().Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);
            var seller = new Business.Models.Seller("159.690.850-57", "Fernando da Silva", "test.test@gmail.com", "123456789");

            // Act
            var sellerService = await _sellerService.Add(seller);


            // Assert 
            Assert.True(sellerService);
            _mocker.GetMock<ISellerRepository>().Verify(s => s.GetById(It.IsAny<Guid>()), Times.Once());
            _mocker.GetMock<ISellerRepository>().Verify(s => s.Add(It.IsAny<Business.Models.Seller>()), Times.Once());
            _mocker.GetMock<ISellerRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once());

        }

        [Fact(DisplayName = "Add new invalid seller")]
        [Trait("Category", "Seller Services")]
        public async void Seller_NewSeller_MustReturnFalse()
        {
            // Arrange
            var seller = new Business.Models.Seller("15985057", "Fernando da Silva", "test.test@gmail.com", "123456789");

            // Act
            var sellerService = await _sellerService.Add(seller);


            // Assert 
            Assert.False(sellerService);
            _mocker.GetMock<ISellerRepository>().Verify(s => s.GetById(It.IsAny<Guid>()), Times.Never());
            _mocker.GetMock<ISellerRepository>().Verify(s => s.Add(It.IsAny<Business.Models.Seller>()), Times.Never());
        }

        [Fact(DisplayName = "Seller already added")]
        [Trait("Category", "Seller Services")]
        public async void Seller_SellerAlreadyAdded_MustReturnFalse()
        {
            // Arrange
            var seller = new Business.Models.Seller("159.690.850-57", "Fernando da Silva", "test.test@gmail.com", "123456789");
            _mocker.GetMock<ISellerRepository>().Setup(r => r.GetById(seller.Id))
                .ReturnsAsync(await Task.FromResult(seller));

            // Act
            var sellerService = await _sellerService.Add(seller);

            // Assert 
            Assert.False(sellerService);
            _mocker.GetMock<ISellerRepository>().Verify(s => s.GetById(It.IsAny<Guid>()), Times.Once());
            _mocker.GetMock<ISellerRepository>().Verify(s => s.Add(It.IsAny<Business.Models.Seller>()), Times.Never());
        }

    }
}
