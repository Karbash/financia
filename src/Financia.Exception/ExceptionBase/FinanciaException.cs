
namespace Financia.Exception.ExceptionBase
{
    public abstract class FinanciaException : SystemException {
        protected FinanciaException(string message) : base(message){}

        public abstract int StatusCode { get; }
        public abstract List<string> GetErrors();
    }
}
