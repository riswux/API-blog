using Microsoft.IdentityModel.Tokens;

namespace WebApplication1.Middleware
{
    public interface ITokenLifetimeManager
    {
        bool ValidateTokenLifetime(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters);
        void SignOut(SecurityToken securityToken);
    }
}
