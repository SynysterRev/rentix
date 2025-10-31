using System.Text.RegularExpressions;

namespace Rentix.Domain.ValueObjects
{
    public sealed class Phone
    {
        public string Value { get; private set; }
        private const string _motif = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";

        private Phone(string value)
        {
            Value = value;
        }

        public static Phone Create(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("A phone number is required");

            if (!IsPhone(input))
                throw new ArgumentException("Invalid phone format");

            return new Phone(input);
        }

        public static bool IsPhone(string? number)
        {
            return number != null && Regex.IsMatch(number, _motif);
        }

        public override string ToString() => Value;
    }
}
