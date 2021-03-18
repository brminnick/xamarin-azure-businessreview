using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;

namespace Reviewer.Core
{
    public class IdentityService : IIdentityService
    {
        public const string ClientID = "adc26e3b-2568-4007-810d-6cc94e7416de";
        public const string RedirectUrl = "msal" + ClientID + "://auth";

        const string Tenant = "b2cbuild.onmicrosoft.com";
        const string SignUpAndInPolicy = "B2C_1_Reviewer_SignUpIn";

        const string AuthorityBase = "https://login.microsoftonline.com/tfp/" + Tenant + "/";
        const string Authority = AuthorityBase + SignUpAndInPolicy;

        static readonly string[] Scopes = { "https://b2cbuild.onmicrosoft.com/reviewer/rvw_all" };

        readonly IPublicClientApplication msaClient = PublicClientApplicationBuilder.Create(ClientID).WithIosKeychainSecurityGroup("com.microsoft.adalcache").WithRedirectUri(RedirectUrl).Build();

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
                msalResult = await msaClient.AcquireTokenInteractive(Scopes).ExecuteAsync();
                if (msalResult?.Account != null)
                {
                    var parsed = ParseIdToken(msalResult.IdToken);
                    DisplayName = parsed["name"]?.ToString() ?? string.Empty;
                }
            }
            catch (MsalServiceException ex)
            {
                if (ex.ErrorCode is "authentication_canceled")
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
                var authResult = await msaClient.AcquireTokenSilent(Scopes, GetUserByPolicy(await msaClient.GetAccountsAsync(), SignUpAndInPolicy)).ExecuteAsync();
                if (authResult?.Account != null)
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

        public async Task Logout()
        {
            var users = await msaClient.GetAccountsAsync();
            foreach (var user in users)
            {
                await msaClient.RemoveAsync(user);
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
