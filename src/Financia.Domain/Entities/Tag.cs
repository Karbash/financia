namespace Financia.Domain.Entities
{
    public class Tag
    {
        public long Id { get; set; }
        public Enuns.Tags Value { get; set; }
        public long ExpenseId { get; set; }
        public Expense Expense { get; set; } = default!;
    }
}
