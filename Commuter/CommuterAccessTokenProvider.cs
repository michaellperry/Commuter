using Commuter.Utilities;
using Newtonsoft.Json.Linq;
using RoverMob.Messaging;
using RoverMob.Tasks;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Windows.Security.Credentials;

namespace Commuter
{
    public class CommuterAccessTokenProvider : Process, IAccessTokenProvider
    {
        private string _accessToken;
        private ImmutableList<TaskCompletionSource<string>> _accessTokenCompletions =
            ImmutableList<TaskCompletionSource<string>>.Empty;

        public CommuterAccessTokenProvider()
        {
        }

        public Task<string> GetAccessTokenAsync()
        {
            if (!String.IsNullOrEmpty(_accessToken))
                return Task.FromResult(_accessToken);

            var completion = new TaskCompletionSource<string>();
            lock (this)
            {
                _accessTokenCompletions = _accessTokenCompletions.Add(
                    completion);
            }
            Authenticate();

            return completion.Task;
        }

        public void RefreshAccessToken()
        {
            _accessToken = null;
        }

        public void Authenticate()
        {
            Perform(async delegate
            {
                await InternalAuthenticateAsync();
            });
        }

        private async Task InternalAuthenticateAsync()
        {
            if (!String.IsNullOrEmpty(_accessToken))
            {
                ReceiveAccessToken(_accessToken);
                return;
            }

            try
            {
                string resource = "https://commuterweb.azurewebsites.net";
                PasswordVault vault = new PasswordVault();
                try
                {
                    var password = vault.Retrieve(resource, "User").Password;
                    ReceiveAccessToken(password);
                }
                catch (Exception)
                {
                    var accessToken = await LogIn(resource);
                    vault.Add(new PasswordCredential(resource, "User", accessToken));
                }
            }
            catch (Exception x)
            {
                ReceiveError(x);
            }
        }

        private async Task<string> LogIn(string resource)
        {
            Uri baseUri = new Uri(resource, UriKind.Absolute);
            var externalLoginsUrl = new Uri(baseUri, "/api/account/externalLogins?returnUrl=/LoggedIn&generateState=true").ToString();
            var providers = await ApiUtility.GetJsonAsync(externalLoginsUrl);
            var providerUrl = providers
                .OfType<JObject>()
                .Select(p => p["Url"].Value<string>())
                .FirstOrDefault();

            var requestUri = new Uri(baseUri, providerUrl);
            var callbackUri = new Uri(baseUri, "/LoggedIn");

            var result = await WebAuthenticationBroker.AuthenticateAsync(
                WebAuthenticationOptions.None,
                requestUri,
                callbackUri);

            if (result.ResponseStatus == WebAuthenticationStatus.Success)
            {
                var parameters = ParseParameters(result.ResponseData);
                if (parameters.Any(p => p[0] == "error"))
                    throw new InvalidOperationException(GetParameter(parameters, "error_description"));
                else
                {
                    string accessToken = GetParameter(parameters, "access_token");
                    if (string.IsNullOrEmpty(accessToken) == false)
                        return ReceiveAccessToken(accessToken);
                    else
                        throw new InvalidOperationException("No access token provided.");
                }
            }
            else if (result.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
            {
                throw new InvalidOperationException("HTTP Error returned by AuthenticateAsync() : " + result.ResponseErrorDetail.ToString());
            }
            else
            {
                throw new InvalidOperationException("Error returned by AuthenticateAsync() : " + result.ResponseStatus.ToString());
            }
        }

        private string ReceiveAccessToken(string accessToken)
        {
            _accessToken = accessToken;
            ImmutableList<TaskCompletionSource<string>> completions;
            lock (this)
            {
                completions = _accessTokenCompletions;
                _accessTokenCompletions = ImmutableList<
                    TaskCompletionSource<string>>.Empty;
            }
            foreach (var completion in completions)
                completion.SetResult(_accessToken);

            return accessToken;
        }

        private void ReceiveError(Exception exception)
        {
            ImmutableList<TaskCompletionSource<string>> completions;
            lock (this)
            {
                completions = _accessTokenCompletions;
                _accessTokenCompletions = ImmutableList<
                    TaskCompletionSource<string>>.Empty;
            }
            foreach (var completion in completions)
                completion.SetException(exception);
        }

        private static string[][] ParseParameters(string responseUrl)
        {
            var parameters = responseUrl
                .Split('#')
                .Skip(1)
                .SelectMany(q => q.Split('&'))
                .Select(p => p.Split('='))
                .Where(p => p.Length >= 2)
                .Select(p => p.Select(t => WebUtility.UrlDecode(t)).ToArray())
                .ToArray();
            return parameters;
        }

        private static string GetParameter(string[][] parameters, string name)
        {
            var accessToken = parameters
                .Where(p => p[0] == name)
                .Select(p => p[1])
                .FirstOrDefault();
            return accessToken;
        }
    }
}
