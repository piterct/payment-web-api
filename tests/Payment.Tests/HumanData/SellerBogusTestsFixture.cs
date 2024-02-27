using Bogus;
using Bogus.Extensions.Brazil;
using Payment.Api.ViewModels;

namespace Payment.Tests.HumanData
{
    [CollectionDefinition(nameof(SellerBogusCollection))]
    public class SellerBogusCollection : ICollectionFixture<SellerBogusTestsFixture>
    { }
    public class SellerBogusTestsFixture
    {
        private readonly Guid _sellerId;
        public SellerBogusTestsFixture()
        {
            _sellerId = Guid.NewGuid();
        }

        public SellerRequest GenerateValidSeller()
        {
            var seller = new Faker<SellerRequest>("pt_BR")
                .CustomInstantiator(f => new SellerRequest(
                    _sellerId.ToString(),
                    f.Person.Cpf(),
                    f.Name.FullName(),
                    f.Person.Email,
                    GeneratePhone(9)
                ));

            return seller;
        }

        public SellerRequest GenerateInvalidSellerPhone()
        {
            var seller = new Faker<SellerRequest>("pt_BR")
                .CustomInstantiator(f => new SellerRequest(
                    _sellerId.ToString(),
                    f.Person.Cpf(),
                    f.Name.FullName(),
                    f.Person.Email,
                    GeneratePhone(5)
                ));

            return seller;
        }

        public SellerRequest GenerateInvalidSellerEmail()
        {
            var seller = new Faker<SellerRequest>("pt_BR")
                .CustomInstantiator(f => new SellerRequest(
                    _sellerId.ToString(),
                    f.Person.Cpf(),
                    f.Name.FullName(),
                    f.Person.Email.Replace(".", "").Replace("@", ""),
                    GeneratePhone(9)
                ));

            return seller;
        }

        public SellerRequest GenerateInvalidSellerName()
        {
            var seller = new Faker<SellerRequest>("pt_BR")
                .CustomInstantiator(f => new SellerRequest(
                    _sellerId.ToString(),
                    f.Person.Cpf(),
                    f.Name.FirstName().Substring(0, 2),
                    f.Person.Email,
                    GeneratePhone(9)
                ));

            return seller;
        }

        public SellerRequest GenerateInvalidSellerId(bool emptyId = false)
        {
            var sellerId = emptyId ? Guid.Empty.ToString() : "51827719-46b1-4573-9d43-318275b2021";

            var seller = new Faker<SellerRequest>("pt_BR")
                .CustomInstantiator(f => new SellerRequest(
                    sellerId,
                    f.Person.Cpf(),
                    f.Name.FullName(),
                    f.Person.Email,
                    GeneratePhone(9)
                ));

            return seller;
        }

        public SellerRequest GenerateNullSellerId()
        {
            var seller = new Faker<SellerRequest>("pt_BR")
                .CustomInstantiator(f => new SellerRequest(
                    null,
                    f.Person.Cpf(),
                    f.Name.FullName(),
                    f.Person.Email,
                    GeneratePhone(9)
                ));

            return seller;
        }

        public string GeneratePhone(int length)
        {
            Random random = new Random();
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
