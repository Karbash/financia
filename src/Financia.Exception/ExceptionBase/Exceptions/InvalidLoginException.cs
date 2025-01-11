using System.Net;

namespace Financia.Exception.ExceptionBase.Exceptions
{
    public class InvalidLoginException : FinanciaException
    {
        public InvalidLoginException() : base(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID)
        {
        }

        public override int StatusCode => (int)HttpStatusCode.Unauthorized;

        public override List<string> GetErrors()
        {
            return [Message];
        }
    }
}
