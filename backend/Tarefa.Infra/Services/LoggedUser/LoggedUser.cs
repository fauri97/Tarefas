using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Tarefa.Domain.Entities;
using Tarefa.Domain.Security.Tokens;
using Tarefa.Domain.Services.LoggedUser;
using Tarefa.Infra.DataAccess;

namespace Tarefa.Infra.Services.LoggedUser
{
    public class LoggedUser(TarefaDbContext context, ITokenProvider tokenProvider) : ILoggedUser
    {
        private readonly TarefaDbContext _context = context;
        private readonly ITokenProvider _tokenProvider = tokenProvider;

        public async Task<User> User()
        {
            var token = _tokenProvider.Value();
            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

            var identifier = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value;
            var userIdentifier = Guid.Parse(identifier);

            return await _context
                .Users
                .AsNoTracking()
                .FirstAsync(user => user.UserIdentifier == userIdentifier);
        }
    }
}