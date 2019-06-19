﻿using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Newtonsoft.Json;

namespace MicrosoftDemoProject
{
    public class MicrosoftAuthService : IMicrosoftAuthService
    {
        private readonly string ClientID = "e2168a1e-d70b-4a72-adb6-2461c6afe4ab";
        private readonly string[] Scopes = { "User.Read" };
        private readonly string GraphUrl = "https://graph.microsoft.com/v1.0/me";

        private IPublicClientApplication publicClientApplication;

        // private bool IsLoaded;
        public void Initialize()
        {
            //if (!this.IsLoaded)
            //{
                this.publicClientApplication = PublicClientApplicationBuilder.Create(ClientID)
                    .WithRedirectUri($"msal{ClientID}://auth")
                    .Build();
            //    this.IsLoaded = true;
            //}
        }

        public static object ParentWindow { get; set; }

        public async Task<User> OnSignInAsync()
        {
            AuthenticationResult authResult = null;
            User currentUser = null;

            var accounts = await this.publicClientApplication.GetAccountsAsync();
            try
            {
                try
                {
                    var firstAccount = accounts.FirstOrDefault();
                    authResult = await this.publicClientApplication.AcquireTokenSilent(Scopes, firstAccount).ExecuteAsync();
                    currentUser = await this.RefreshUserDataAsync(authResult?.AccessToken).ConfigureAwait(false);
                }
                catch (MsalUiRequiredException ex)
                {
                    // the user was not already connected.
                    try
                    {
                        authResult = await this.publicClientApplication.AcquireTokenInteractive(Scopes)
                                                    .WithParentActivityOrWindow(ParentWindow)
                                                    .ExecuteAsync();
                        currentUser = await this.RefreshUserDataAsync(authResult?.AccessToken).ConfigureAwait(false);
                    }
                    catch (Exception ex2)
                    {
                        // Manage the exception with a logger as you need
                        System.Diagnostics.Debug.WriteLine(ex2.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                // Manage the exception with a logger as you need
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }

            return currentUser;
        }

        public async Task OnSignOutAsync()
        {
            var accounts = await this.publicClientApplication.GetAccountsAsync();
            try
            {
                while (accounts.Any())
                {
                    await this.publicClientApplication.RemoveAsync(accounts.FirstOrDefault());
                    accounts = await this.publicClientApplication.GetAccountsAsync();
                }
            }
            catch (Exception ex)
            {
                // Manage the exception with a logger as you need
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Refresh user date from the Graph api.
        /// </summary>
        /// <param name="token">The user access token.</param>
        /// <returns>The current user with his associated informations.</returns>
        private async Task<User> RefreshUserDataAsync(string token)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, this.GraphUrl);
            message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
            HttpResponseMessage response = await client.SendAsync(message);
            User currentUser = null;

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                currentUser = JsonConvert.DeserializeObject<User>(json);
            }

            return currentUser;
        }
    }
}