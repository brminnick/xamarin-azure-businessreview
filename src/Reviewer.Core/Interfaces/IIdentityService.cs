using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace Reviewer.Core
{
    public interface IIdentityService
    {
        string DisplayName { get; set; }

        Task<AuthenticationResult?> Login();
        Task<AuthenticationResult?> GetCachedSignInToken();
        void Logout();
    }
}
