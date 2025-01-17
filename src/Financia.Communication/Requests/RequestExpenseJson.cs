
using Financia.Communication.Enums;

namespace Financia.Communication.Requests
{
    public class RequestExpenseJson
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public decimal Amount {  get; set; }
        public PaymentType Payment {  get; set; }
        public IList<Tag> Tags { get; set; } = [];
    }
}
