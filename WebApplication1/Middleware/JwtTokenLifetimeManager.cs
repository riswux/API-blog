using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;

namespace WebApplication1.Middleware
{
    public class JwtTokenLifetimeManager : ITokenLifetimeManager
    {
        private static readonly ConcurrentDictionary<string, DateTime> DisavowedSignatures = new();

        public bool ValidateTokenLifetime(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (securityToken is JwtSecurityToken token)
            {
                return token.ValidFrom <= DateTime.UtcNow &&
                       token.ValidTo >= DateTime.UtcNow &&
                       !DisavowedSignatures.ContainsKey(token.RawSignature);
            }

            return false;
        }

        public void SignOut(SecurityToken securityToken)
        {
            if (securityToken is JwtSecurityToken token)
            {
                DisavowedSignatures.TryAdd(token.RawSignature, token.ValidTo);

                foreach (var key in DisavowedSignatures.Keys.Where(key => DisavowedSignatures[key] < DateTime.UtcNow).ToList())
                {
                    DisavowedSignatures.TryRemove(key, out _);
                }
            }
        }
    }
}
