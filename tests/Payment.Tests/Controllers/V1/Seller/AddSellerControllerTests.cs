using AutoMapper;
using Moq;
using Moq.AutoMock;
using Payment.Api.Controllers.V1;
using Payment.Api.ViewModels;
using Payment.Business.Interfaces.Services;
using Payment.Tests.HumanData;
using System.ComponentModel.DataAnnotations;

namespace Payment.Tests.Controllers.V1.Seller
{
    [Collection(nameof(SellerBogusCollection))]
    public class AddSellerControllerTests
    {
        private readonly SellerBogusTestsFixture _sellerBogusFixture;
        private readonly AutoMocker _mocker;
        private readonly SellerController _sellerController;
        private readonly List<ValidationResult> _validationResults;

        public AddSellerControllerTests(SellerBogusTestsFixture sellerBogusFixture)
        {
            _sellerBogusFixture = sellerBogusFixture;
            _mocker = new AutoMocker();
            _sellerController = _mocker.CreateInstance<SellerController>();
            _validationResults = new List<ValidationResult>();
        }

        [Fact(DisplayName = "Add new valid seller")]
        [Trait("Category", "Seller Controllers")]
        public async void Seller_NewSeller_MustBeValid()
        {
            // Arrange
            var sellerRequest = _sellerBogusFixture.GenerateValidSeller();
            var seller = new Business.Models.Seller(Guid.Parse(sellerRequest.Id), sellerRequest.Cpf, sellerRequest.Name, sellerRequest.Email, sellerRequest.Phone);
            _mocker.GetMock<IMapper>().Setup(m => m.Map<Business.Models.Seller>(It.IsAny<SellerRequest>())).Returns(seller);

            // Act
            await _sellerController.AddSeller(sellerRequest);
            var isValid = Validator.TryValidateObject(sellerRequest, new ValidationContext(sellerRequest), _validationResults, true);


            // Assert 
            Assert.True(isValid);
            _mocker.GetMock<ISellerService>().Verify(s => s.Add(It.IsAny<Business.Models.Seller>()), Times.Once());
        }

        [Fact(DisplayName = "Add new valid seller  with the same id")]
        [Trait("Category", "Seller Controllers")]
        public async void Seller_ValidSeller_MustBeWithTheSameId()
        {
            // Arrange
            var sellerRequest = _sellerBogusFixture.GenerateValidSeller();
            var seller = new Business.Models.Seller(Guid.Parse(sellerRequest.Id), sellerRequest.Cpf, sellerRequest.Name, sellerRequest.Email, sellerRequest.Phone);

            // Act
            await _sellerController.AddSeller(sellerRequest);
            var isValid = Validator.TryValidateObject(sellerRequest, new ValidationContext(sellerRequest), _validationResults, true);


            // Assert 
            Assert.True(isValid);
            Assert.Equal(sellerRequest.Id.ToString(), seller.Id.ToString());
        }

        [Fact(DisplayName = "Add new seller with invalid  empty GUID id")]
        [Trait("Category", "Seller Controllers")]
        public async void Seller_InvalidSeller_IdMustBeEmpty()
        {
            // Arrange
            var sellerRequest = _sellerBogusFixture.GenerateInvalidSellerId(true);
            var seller = new Business.Models.Seller(Guid.Parse(sellerRequest.Id), sellerRequest.Cpf, sellerRequest.Name, sellerRequest.Email, sellerRequest.Phone);


            // Act
            var isValid = Validator.TryValidateObject(sellerRequest, new ValidationContext(sellerRequest), _validationResults, true);


            // Assert 
            Assert.False(isValid);
            Assert.Equal(sellerRequest.Id.ToString(), seller.Id.ToString());
            Assert.Equal(Guid.Empty.ToString(), sellerRequest.Id.ToString());
        }

        [Fact(DisplayName = "Add new seller with invalid GUID id")]
        [Trait("Category", "Seller Controllers")]
        public async void Seller_InvalidSeller_IdGuidMustBeInvalid()
        {
            // Arrange
            var sellerRequest = _sellerBogusFixture.GenerateInvalidSellerId(false);

            // Act
            var isValid = Validator.TryValidateObject(sellerRequest, new ValidationContext(sellerRequest), _validationResults, true);

            // Assert 
            Assert.False(isValid);
        }

        [Fact(DisplayName = "Add new seller with invalid null GUID id")]
        [Trait("Category", "Seller Controllers")]
        public async void Seller_InvalidSeller_IdGuidNullMustBeInvalid()
        {
            // Arrange
            var sellerRequest = _sellerBogusFixture.GenerateNullSellerId();

            // Act
            var isValid = Validator.TryValidateObject(sellerRequest, new ValidationContext(sellerRequest), _validationResults, true);

            // Assert 
            Assert.False(isValid);
        }

        [Fact(DisplayName = "Add new seller with invalid name")]
        [Trait("Category", "Seller Controllers")]
        public async void Seller_InvalidSeller_NameMustBeInvalid()
        {
            // Arrange
            var sellerRequest = _sellerBogusFixture.GenerateInvalidSellerName();

            // Act
            var isValid = Validator.TryValidateObject(sellerRequest, new ValidationContext(sellerRequest), _validationResults, true);

            // Assert 
            Assert.False(isValid);
        }

        [Fact(DisplayName = "Add new seller with invalid email ")]
        [Trait("Category", "Seller Controllers")]
        public async void Seller_InvalidSeller_EmailMustBeInvalid()
        {
            // Arrange
            var sellerRequest = _sellerBogusFixture.GenerateInvalidSellerEmail();

            // Act
            var isValid = Validator.TryValidateObject(sellerRequest, new ValidationContext(sellerRequest), _validationResults, true);

            // Assert 
            Assert.False(isValid);
        }

        [Fact(DisplayName = "Add new seller with invalid phone")]
        [Trait("Category", "Seller Controllers")]
        public async void Seller_InvalidSeller_PhoneMustBeInvalid()
        {
            // Arrange
            var sellerRequest = _sellerBogusFixture.GenerateInvalidSellerPhone();

            // Act
            var isValid = Validator.TryValidateObject(sellerRequest, new ValidationContext(sellerRequest), _validationResults, true);

            // Assert 
            Assert.False(isValid);
        }
    }
}
