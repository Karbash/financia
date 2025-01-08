using Bogus;
using Financia.Communication.Enums;
using Financia.Communication.Requests;

namespace CommomTestUtilities.Requests
{
    public static class RequestRegisterExpenseJsonBuilder
    {
        public static RequestExpenseJson Build() 
        {
            var faker = new Faker("en");

            return new RequestExpenseJson
            {
                Title = faker.Commerce.Product(),
                Amount = faker.Random.Decimal( min: 1, max: 1000 ),
                Date = faker.Date.Past(),
                Description = faker.Commerce.ProductDescription(),
                Payment = faker.PickRandom<PaymentType>()
            };
        }
    } 
}
