using Financia.Communication.Requests;
using Financia.Exception;
using FluentValidation;

namespace Financia.Application.UseCases.Expenses
{
    public class ExpenseValidator : AbstractValidator<RequestExpenseJson>
    {
        public ExpenseValidator()
        {
            RuleFor(expense => expense.Title)
                .NotEmpty()
                .WithMessage(ResourceErrorMessages.TITLE_REQUIRED);
            RuleFor(expense => expense.Amount)
                .GreaterThan(0)
                .WithMessage(ResourceErrorMessages.AMOUNT_GREATER_ZERO);
            RuleFor(expense => expense.Date)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage(ResourceErrorMessages.EXPENSE_NOT_FUTURE);
            RuleFor(expense => expense.Payment)
                .IsInEnum()
                .WithMessage(ResourceErrorMessages.PAYMENT_TYPE_INVALID);
        }
    }
}
