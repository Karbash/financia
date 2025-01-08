
using AutoMapper;
using Financia.Communication.Requests;
using Financia.Communication.Responses;
using Financia.Domain.Entities;
using Financia.Domain.Enuns;
using Financia.Domain.Repositories;
using Financia.Domain.Repositories.Expenses;
using Financia.Exception.ExceptionBase.Exceptions;

namespace Financia.Application.UseCases.Expenses.Register
{
    public class RegisterExpenseUseCase: IRegisterExpenseUseCase
    {
        private readonly IExpensesWriteOnlyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RegisterExpenseUseCase(
            IExpensesWriteOnlyRepository expensesRepository, 
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _repository = expensesRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseRegisterExpenseJson> Execute(RequestExpenseJson request)
        {
            Validate(request);

            Expense entity = _mapper.Map<Expense>(request);

            await _repository.Add(entity);
            await _unitOfWork.Commit();

            return _mapper.Map<ResponseRegisterExpenseJson>(entity);
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
