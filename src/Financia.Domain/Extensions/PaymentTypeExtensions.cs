using Financia.Domain.Enuns;
using Financia.Domain.Reports;

namespace Financia.Domain.Extensions
{
    public static class PaymentTypeExtensions
    {
        public static string PaymentTypeToString(this PaymentType paymentType)
        {
            return paymentType switch
            {
                PaymentType.CreditCard => ResourceReportGenerationMessages.CREDITCARD,
                PaymentType.Cash => ResourceReportGenerationMessages.CASH,
                PaymentType.EletronicTransfer => ResourceReportGenerationMessages.ELETRONICTRANSFER,
                PaymentType.DebitCard => ResourceReportGenerationMessages.DEBITCARD,
                _ => string.Empty
            };
        }
    }
}
