using System;
using Microsoft.Identity.Client;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Reviewer.Core;
using System.Linq;

[assembly: Dependency(typeof(IdentityService))]
namespace Reviewer.Core
{
    public class IdentityService : IIdentityService
    {
        static readonly string Tenant = "b2cbuild.onmicrosoft.com";
        static readonly string ClientID = "adc26e3b-2568-4007-810d-6cc94e7416de";
        static readonly string SignUpAndInPolicy = "B2C_1_Reviewer_SignUpIn";

        static readonly string AuthorityBase = $"https://login.microsoftonline.com/tfp/{Tenant}/";
        static readonly string Authority = $"{AuthorityBase}{SignUpAndInPolicy}";

        static readonly string[] Scopes = { "https://b2cbuild.onmicrosoft.com/reviewer/rvw_all" };

        static readonly string RedirectUrl = $"msal{ClientID}://auth";

        readonly IPublicClientApplication msaClient = PublicClientApplicationBuilder.Create(ClientID).WithRedirectUri(RedirectUrl).Build();

        public string DisplayName { get; set; } = string.Empty;

        public async Task<AuthenticationResult?> Login()
        {
            // First check if the token happens to be cached - grab silently
            var msalResult = await GetCachedSignInToken();
            if (msalResult != null)
                return msalResult;

            // Token not in cache - call adb2c to acquire it
            try
            {
                msalResult = await msaClient.AcquireTokenAsync(Scopes,
                                                           GetUserByPolicy(msaClient.Users,
                                                                           SignUpAndInPolicy),
                                                           UIBehavior.ForceLogin,
                                                           null,
                                                           null,
                                                           Authority,
                                                           UIParent);
                if (msalResult?.User != null)
                {
                    var parsed = ParseIdToken(msalResult.IdToken);
                    DisplayName = parsed["name"]?.ToString() ?? string.Empty;
                }
            }
            catch (MsalServiceException ex)
            {
                if (ex.ErrorCode == MsalClientException.AuthenticationCanceledError)
                {
                    System.Diagnostics.Debug.WriteLine("User cancelled");
                }
                else if (ex.ErrorCode == "access_denied")
                {
                    // most likely the forgot password was hit
                    System.Diagnostics.Debug.WriteLine("Forgot password");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return msalResult;
        }

        public async Task<AuthenticationResult?> GetCachedSignInToken()
        {
            try
            {
                // This checks to see if there's already a user in the cache
                var authResult = await msaClient.AcquireTokenSilentAsync(Scopes,
                                                               GetUserByPolicy(msaClient.Users,
                                                                               SignUpAndInPolicy),
                                                               Authority,
                                                               false);
                if (authResult?.User != null)
                {
                    var parsed = ParseIdToken(authResult.IdToken);
                    DisplayName = parsed["name"]?.ToString() ?? string.Empty;
                }

                return authResult;
            }
            catch (MsalUiRequiredException ex)
            {
                // happens if the user hasn't logged in yet & isn't in the cache
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return null;
        }

        public void Logout()
        {
            foreach (var user in msaClient.Users)
            {
                msaClient.Remove(user);
            }
        }

        IAccount? GetUserByPolicy(IEnumerable<IAccount> users, string policy)
        {
            foreach (var user in users)
            {
                string userIdentifier = Base64UrlDecode(user.HomeAccountId.Identifier.Split('.')[0]);

                if (userIdentifier.EndsWith(policy.ToLower(), StringComparison.OrdinalIgnoreCase)) return user;
            }

            return null;
        }

        string Base64UrlDecode(string s)
        {
            s = s.Replace('-', '+').Replace('_', '/');
            s = s.PadRight(s.Length + (4 - s.Length % 4) % 4, '=');
            var byteArray = Convert.FromBase64String(s);
            var decoded = Encoding.UTF8.GetString(byteArray, 0, byteArray.Count());
            return decoded;
        }

        JObject ParseIdToken(string idToken)
        {
            // Get the piece with actual user info
            idToken = idToken.Split('.')[1];
            idToken = Base64UrlDecode(idToken);
            return JObject.Parse(idToken);
        }
    }
}
