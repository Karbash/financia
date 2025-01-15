using Financia.Application.AutoMapper;
using Financia.Application.UseCases.Expenses.ById;
using Financia.Application.UseCases.Expenses.Delete;
using Financia.Application.UseCases.Expenses.GetAll;
using Financia.Application.UseCases.Expenses.Register;
using Financia.Application.UseCases.Expenses.Reports.Excel;
using Financia.Application.UseCases.Expenses.Reports.Pdf;
using Financia.Application.UseCases.Expenses.Update;
using Financia.Application.UseCases.Login.DoLogin;
using Financia.Application.UseCases.Users.Delete;
using Financia.Application.UseCases.Users.GetProfile;
using Financia.Application.UseCases.Users.PasswordChange;
using Financia.Application.UseCases.Users.Register;
using Financia.Application.UseCases.Users.UpdateProfile;
using Microsoft.Extensions.DependencyInjection;

namespace Financia.Application
{
    public static class DependencyInjectionExtension
    {
        public static void AddApplication(this IServiceCollection services)
        {
            AddAutoMapper(services);
            AddUseCases(services);
        }

        private static void AddAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapping));
        }

        private static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<IRegisterExpenseUseCase, RegisterExpenseUseCase>();
            services.AddScoped<IGetAllExpensesUseCase, GetAllExpensesUseCase>();
            services.AddScoped<IGetExpenseByIdUseCase, GetExpenseByIdUseCase>();
            services.AddScoped<IDeleteExpenseUseCase, DeleteExpenseUseCase>();
            services.AddScoped<IUpdateExpenseUseCase, UpdateExpenseUseCase>();
            services.AddScoped<IGenerateExpensesReportExcelUseCase, GenerateExpensesReportExcelUseCase>();
            services.AddScoped<IGenerateExpensesReportPdfUseCase, GenerateExpensesReportPdfUseCase>();
            services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
            services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
            services.AddScoped<IGetProfileUseCase, GetProfileUseCase>();
            services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
            services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
            services.AddScoped<IDeleteUserAccountUseCase, DeleteUserAccountUseCase>();
        }
    }
}
