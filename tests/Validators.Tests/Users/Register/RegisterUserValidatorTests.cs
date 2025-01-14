using CommomTestUtilities.Requests;
using Financia.Application.UseCases.Users.Register;
using Financia.Exception;

namespace Validators.Tests.Users.Register
{
    public class RegisterUserValidatorTests
    {
        [Fact]
        public void Success()
        {
            //Arange
            var validator = new RegisterUserValidator();
            var request = RequestRegisterUserJsonBuilder.Build();
            //Act
            var result = validator.Validate(request);
            //Assert
            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData("     ")]
        [InlineData(null)]
        public void NameEmpty(string name)
        {
            //Arange
            var validator = new RegisterUserValidator();
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = name;
            //Act
            var result = validator.Validate(request);
            //Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == ResourceErrorMessages.NAME_EMPTY);
        }

        [Theory]
        [InlineData("")]
        [InlineData("     ")]
        [InlineData(null)]
        public void EmailEmpty(string email)
        {
             //Arange
             var validator = new RegisterUserValidator();
             var request = RequestRegisterUserJsonBuilder.Build();
             request.Email = email;
             //Act
             var result = validator.Validate(request);
             //Assert
             Assert.False(result.IsValid);
             Assert.Contains(result.Errors, error => error.ErrorMessage == ResourceErrorMessages.EMAIL_EMPTY);
        }

        [Theory]
        [InlineData("email.com.br")]
        [InlineData("emailbr")]
        [InlineData("emailbr@")]
        [InlineData("@emailbr@")]
        [InlineData("@emailbr")]
        public void EmailInvalid(string email)
        {
            //Arange
            var validator = new RegisterUserValidator();
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Email = email;
            //Act
            var result = validator.Validate(request);
            //Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == ResourceErrorMessages.EMAIL_INVALID);
        }

        [Theory]
        [InlineData("mamhv3")]
        [InlineData("Mamherujgv")]
        [InlineData("123!ddddddd@")]
        [InlineData("@Oallllll@")]
        public void PasswordInvalid(string password)
        {
            //Arange
            var validator = new RegisterUserValidator();
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Password = password;
            //Act
            var result = validator.Validate(request);
            //Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == ResourceErrorMessages.PASSWORD_INVALID);
        }

    }
}
