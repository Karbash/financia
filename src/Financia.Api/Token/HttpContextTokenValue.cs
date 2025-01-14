using Financia.Domain.Security.Tokens;

namespace Financia.Api.Token
{
    public class HttpContextTokenValue : ITokenProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HttpContextTokenValue(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string TokenOnRequest()
        {
            var autorization =  _httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString();
            return autorization.Replace("Bearer ", "");
        }
    }
}
