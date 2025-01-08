
namespace Financia.Communication.Responses.Error
{
    public class ResponseErrorJson
    {
        public List<string> ErrorMessages { get; set; } = [];

        public ResponseErrorJson(List<string> errorMessages)
        {
            ErrorMessages = errorMessages;
        }

        public ResponseErrorJson(string errorMessage)
        {
            ErrorMessages.Add(errorMessage);
        }
    }
}
