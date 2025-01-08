using System.Net;

namespace Financia.Exception.ExceptionBase.Exceptions
{
    public class NotFoundException : FinanciaException
    {
        public NotFoundException(string message) : base(message){}

        public override int StatusCode => (int)HttpStatusCode.NotFound;

        public override List<string> GetErrors()
        {
            return new List<string>() { Message };
        }
    }
}
