using System.Net.Mail;

namespace Rentix.Domain.ValueObjects
{
    public sealed class Email
    {
        public string Value { get; private set; }

        private Email(string value)
        {
            Value = value;
        }

        public static Email Create(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Email is required");

            if (!IsEmailAddress(input))
                throw new ArgumentException("Invalid email format");

            return new Email(input);
        }

        public static bool IsEmailAddress(string? input)
        {
            return MailAddress.TryCreate(input, out var mailAddress) &&
                   mailAddress.Address == input;
        }

        public override string ToString() => Value;
    }
}
