namespace Payment.Business.Models
{
    public class Seller : Entity
    {
        public Seller(string cpf, string name, string email, string phone)
        {
            Cpf = cpf;
            Name = name;
            Email = email;
            Phone = phone;
        }

        public Seller(Guid id, string cpf, string name, string email, string phone)
        {
            Id = id;
            Cpf = cpf;
            Name = name;
            Email = email;
            Phone = phone;
        }

        public Seller()
        { }

        public string Cpf { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Phone { get; private set; } = string.Empty;

    }
}
