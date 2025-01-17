using Financia.Communication.Enums;

namespace Financia.Communication.Responses
{
    public class ResponseExpenseJson
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime Date { get; set; }  
        public decimal Amount { get; set; } 
        public PaymentType Payment { get; set; }
        public IList<Tag> Tags { get; set; } = new List<Tag>();
    }
}
