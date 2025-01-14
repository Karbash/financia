
using AutoMapper;
using Financia.Communication.Requests;
using Financia.Communication.Responses;
using Financia.Domain.Entities;
using Financia.Domain.Enuns;
using Financia.Domain.Repositories;
using Financia.Domain.Repositories.Expenses;
using Financia.Domain.Services.LoggedUser;
using Financia.Exception.ExceptionBase.Exceptions;

namespace Financia.Application.UseCases.Expenses.Register
{
    public class RegisterExpenseUseCase: IRegisterExpenseUseCase
    {
        private readonly IExpensesWriteOnlyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;

        public RegisterExpenseUseCase(
            IExpensesWriteOnlyRepository expensesRepository, 
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILoggedUser loggedUser
            )
        {
            _repository = expensesRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggedUser = loggedUser;
        }

        public async Task<ResponseRegisterExpenseJson> Execute(RequestExpenseJson request)
        {
            Validate(request);
            
            var loggedUser = await _loggedUser.Get();

            Expense expense = _mapper.Map<Expense>(request);

            expense.UserId = loggedUser.Id;

            await _repository.Add(expense);
            await _unitOfWork.Commit();

            return _mapper.Map<ResponseRegisterExpenseJson>(expense);
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
