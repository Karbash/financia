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
            CreateMap<RequestRegisterUserJson, User>()
                .ForMember(dest => dest.Password, config => config.Ignore());

            CreateMap<RequestExpenseJson, Expense>()
                .ForMember(dest => dest.Tags, config => config.MapFrom(source => source.Tags.Distinct()));

            CreateMap<Communication.Enums.Tag, Tag>()
                .ForMember(dest => dest.Value, config => config.MapFrom(source => source));
        }

        private void EntityToResponse() 
        {
            CreateMap<Expense, ResponseExpenseJson>()
                .ForMember(dest => dest.Tags, config => config.MapFrom(source => source.Tags.Select( tag => tag.Value)));
            CreateMap<Expense, ResponseRegisterExpenseJson>();
            CreateMap<Expense, ResponseShortExpenseJson>();
            CreateMap<User, ResponseRegisterUserJson>();
            CreateMap<User, ResponseUserProfileJson>();
        }
    }
}
