using Financia.Application.UseCases.Users;
using Financia.Communication.Requests;
using FluentValidation;

namespace Validators.Tests.Users
{
    public class PasswordValidatorTest
    {
        [Theory]
        [InlineData("mamhv3")]
        [InlineData("Mamherujgv")]
        [InlineData("123!ddddddd@")]
        [InlineData("@Oallllll@")]
        public void PasswordInvalid(string password)
        {
            //Arange
            var validator = new PasswordValidator<RequestRegisterUserJson>();
            //Act
            var result = validator.IsValid(new ValidationContext<RequestRegisterUserJson>( new RequestRegisterUserJson()) ,password);
            //Assert
            Assert.False(result);
           
        }
    }
}
