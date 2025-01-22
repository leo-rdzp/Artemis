using System.Security.Claims;

namespace Artemis.Backend.IServices.Authentication
{
    public interface ISecurityTokenValidator
    {
        Task<bool> ValidatePrincipalAsync(ClaimsPrincipal? principal);
    }
}
