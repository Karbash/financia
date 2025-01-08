using System.Globalization;

namespace Financia.Api.Middleware
{
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;

        public CultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            List<CultureInfo> supportedLanguages = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();
            var requestCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();

            var cultureInfo = new CultureInfo("en");

            if (string.IsNullOrWhiteSpace(requestCulture) == false 
                && supportedLanguages.Exists(language => language.Name.Equals(requestCulture)))
            {
                cultureInfo = new CultureInfo(requestCulture);
            }

            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;

            await _next(context);
        }
    }
}
