using Payment.Business.Helpers;

namespace Payment.Business.Validations.Documents
{
    public class CpfValidationDocs
    {
        public const int CpfSize = 11;
        public static bool Validate(string cpf)
        {
            var cpfNumbers = NumberHelper.OnlyNumbers(cpf);

            if (!ValidSize(cpfNumbers)) return false;
            return !HasRepeatedDigits(cpfNumbers) && HasValidDigits(cpfNumbers);
        }

        private static bool ValidSize(string value)
        {
            return value.Length == CpfSize;
        }
        private static bool HasRepeatedDigits(string value)
        {
            string[] invalidNumbers =
            {
                "00000000000",
                "11111111111",
                "22222222222",
                "33333333333",
                "44444444444",
                "55555555555",
                "66666666666",
                "77777777777",
                "88888888888",
                "99999999999"
            };
            return invalidNumbers.Contains(value);
        }

        private static bool HasValidDigits(string valor)
        {
            var number = valor.Substring(0, CpfSize - 2);
            var checkDigit = new VerificationDigitHelper(number)
                .WithMultipliersFromTo(2, 11)
                .Substituting("0", 10, 11);
            var firstDigit = checkDigit.CalculateDigit();
            checkDigit.AddDigit(firstDigit);
            var secondDigit = checkDigit.CalculateDigit();

            return string.Concat(firstDigit, secondDigit) == valor.Substring(CpfSize - 2, 2);
        }
    }
}
