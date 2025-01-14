using Financia.Domain.Entities;
using Financia.Domain.Security.Tokens;
using Financia.Domain.Services.LoggedUser;
using Financia.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Financia.Infrastructure.Services.LoggedUser
{
    internal class LoggedUser : ILoggedUser
    {
        private readonly FinanciaDBContext _financiaDBContext;
        private readonly ITokenProvider _tokenProvider;

        public LoggedUser(FinanciaDBContext financiaDBContext, ITokenProvider tokenProvider)
        {
            _financiaDBContext = financiaDBContext;
            _tokenProvider = tokenProvider;
        }

        public async Task<User> Get()
        {
            string token = _tokenProvider.TokenOnRequest();

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
            var indentifier = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value;

            var user = await _financiaDBContext
                .Users
                .AsNoTracking()
                .FirstAsync(user => user.UserIdentifier == Guid.Parse(indentifier));
            return user;
        }
    }
}
