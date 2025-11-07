using Rentix.Domain.ValueObjects;
using Xunit;

namespace Rentix.Tests.Unit.Domain.ValueObjects
{
    public class ValueObjectTests
    {
        [Theory]
        [InlineData("john@doe.com")]
        [InlineData("user.name+tag@domain.fr")]
        [InlineData("user@sub.domain.com")]
        public void Email_IsEmailAddress_ShouldReturnTrue_ForValidEmails(string email)
        {
            Assert.True(Email.IsEmailAddress(email));
        }

        [Theory]
        [InlineData("")]
        [InlineData("notanemail")]
        [InlineData("user@.com")]
        [InlineData(null)]
        public void Email_IsEmailAddress_ShouldReturnFalse_ForInvalidEmails(string? email)
        {
            Assert.False(Email.IsEmailAddress(email));
        }

        [Theory]
        [InlineData("0601020304")]
        [InlineData("0102030405")]
        public void Phone_IsPhone_ShouldReturnTrue_ForValidPhones(string phone)
        {
            Assert.True(Phone.IsPhone(phone));
        }

        [Theory]
        [InlineData("")]
        [InlineData("notaphone")]
        [InlineData("123")]
        [InlineData(null)]
        public void Phone_IsPhone_ShouldReturnFalse_ForInvalidPhones(string? phone)
        {
            Assert.False(Phone.IsPhone(phone));
        }

        [Fact]
        public void Email_Create_ShouldThrow_ForInvalidEmail()
        {
            Assert.ThrowsAny<System.Exception>(() => Email.Create("notanemail"));
        }

        [Fact]
        public void Phone_Create_ShouldThrow_ForInvalidPhone()
        {
            Assert.ThrowsAny<System.Exception>(() => Phone.Create("notaphone"));
        }

        [Fact]
        public void Email_Create_ShouldReturnEmail_ForValidEmail()
        {
            var email = Email.Create("john@doe.com");
            Assert.Equal("john@doe.com", email.Value);
        }

        [Fact]
        public void Phone_Create_ShouldReturnPhone_ForValidPhone()
        {
            var phone = Phone.Create("0601020304");
            Assert.Equal("0601020304", phone.Value);
        }
    }
}
