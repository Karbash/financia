using AutoMapper;
using Financia.Communication.Requests;
using Financia.Communication.Responses;
using Financia.Domain.Entities;

namespace Financia.Application.AutoMapper
{
    public class AutoMapping: Profile
    {
        public AutoMapping() 
        {
            RequestToEntity();
            EntityToResponse();
        }

        private void RequestToEntity() 
        {
            CreateMap<RequestExpenseJson, Expense>();
            CreateMap<ResponseRegisterUserJson, User>();
        }

        private void EntityToResponse() 
        {
            CreateMap<Expense, ResponseRegisterExpenseJson>();
            CreateMap<Expense, ResponseShortExpenseJson>();
            CreateMap<Expense, ResponseExpenseJson>();
            CreateMap<User, ResponseRegisterUserJson>();
        }
    }
}
