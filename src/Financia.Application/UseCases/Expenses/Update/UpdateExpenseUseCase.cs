using Financia.Communication.Requests;
using Financia.Domain.Repositories.Expenses;
using Financia.Domain.Repositories;
using Financia.Application.UseCases.Expenses.Register;
using Financia.Exception.ExceptionBase.Exceptions;
using AutoMapper;
using Financia.Exception;
using Financia.Domain.Entities;
using Financia.Domain.Services.LoggedUser;

namespace Financia.Application.UseCases.Expenses.Update
{
    public class UpdateExpenseUseCase : IUpdateExpenseUseCase
    {
        private readonly IExpensesUpdateOnlyRepository _expenseRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;

        public UpdateExpenseUseCase(
            IExpensesUpdateOnlyRepository expenseRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILoggedUser loggedUser
            )
        {
            _expenseRepository = expenseRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggedUser = loggedUser;
        }

        public async Task Execute(long id, RequestExpenseJson request)
        {
            Validate(request);

            var loggedUser = await _loggedUser.Get();
            var expense = await _expenseRepository.GetById(loggedUser, id );

            if (expense is null)
            {
                throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
            }

            _mapper.Map(request, expense);
           
            _expenseRepository.Update(expense);

            await _unitOfWork.Commit();
        }

        private void Validate(RequestExpenseJson request)
        {
            var validator = new ExpenseValidator();
            var result = validator.Validate(request);

            if (result.IsValid == false)
            {
                var errorMessages = result.Errors
                                    .Select(f => f.ErrorMessage)
                                    .ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
