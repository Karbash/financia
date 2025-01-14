using CommomTestUtilities.Requests;
using FluentAssertions;
using Financia.Exception;
using Financia.Application.UseCases.Expenses;
using Financia.Communication.Requests;

namespace Validators.Tests;

public class RegisterExpenseValidatorTests
{
    [Fact]
    public void Success()
    {
        //Arrange
        ExpenseValidator validator = new ExpenseValidator();
        RequestExpenseJson request = RequestRegisterExpenseJsonBuilder.Build();
        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("            ")]
    [InlineData(null)]

    public void Error_Title_Empty(string title)
    {
        //Arrange
        ExpenseValidator validator = new ExpenseValidator();
        RequestExpenseJson request = RequestRegisterExpenseJsonBuilder.Build();

        request.Title = title;

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.TITLE_REQUIRED));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-2)]
    [InlineData(-10)]
    public void Error_Amount_Invalid(decimal amount)
    {
        //Arrange
        ExpenseValidator validator = new ExpenseValidator();
        RequestExpenseJson request = RequestRegisterExpenseJsonBuilder.Build();

        request.Amount = amount;

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.AMOUNT_GREATER_ZERO));
    }

    [Fact]
    public void Error_Date_Not_In_Future()
    {
        //Arrange
        ExpenseValidator validator = new ExpenseValidator();
        RequestExpenseJson request = RequestRegisterExpenseJsonBuilder.Build();

        request.Date = DateTime.Now.AddDays(+1);

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EXPENSE_NOT_FUTURE));
    }

    [Fact]
    public void Error_Payment_Type_Invalid()
    {
        //Arrange
        ExpenseValidator validator = new ExpenseValidator();
        RequestExpenseJson request = RequestRegisterExpenseJsonBuilder.Build();

        request.Payment = (Financia.Communication.Enums.PaymentType) 9;

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.PAYMENT_TYPE_INVALID));
    }
}
