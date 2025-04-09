using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PEI_API_Assesment
{
    public class TokenClient
    {
        private readonly string _tenantId = "7de331ba-96a3-4983-99c3-2c19ad816340";
        private readonly string _clientId = "d04fd0d3-7f1b-4601-9564-0d9b92dccd87";
        private readonly string _clientSecret = "zTJ8Q~QE2DgnfpIqZarSsS6KsP6If2-_~hcmnaT3";
        private readonly string _scope = "https://org10da1532.crm8.dynamics.com/.default";

       
        public async Task<string> GetBearerTokenAsync()
        {
            var options = new RestClientOptions("https://login.microsoftonline.com")
            {
                MaxTimeout = -1,
            };

            var client = new RestClient(options);
            var request = new RestRequest($"/{_tenantId}/oauth2/v2.0/token", Method.Post);

            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            // You can remove or replace this cookie header if not needed
            request.AddHeader("Cookie", "fpc=placeholder-cookie");

            request.AddParameter("client_id", _clientId);
            request.AddParameter("client_secret", _clientSecret);
            request.AddParameter("grant_type", "client_credentials");
            request.AddParameter("scope", _scope);

            RestResponse response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Token request failed: {response.StatusCode} - {response.Content}");
            }

            Console.WriteLine("Access Token Response:");
            Console.WriteLine(response.Content);
            Console.WriteLine(response.StatusCode);
            OAuthTokenResponse oAuthTokenResponse = JsonConvert.DeserializeObject<OAuthTokenResponse>(response.Content);
            string ExtractedAccessToekn = oAuthTokenResponse.access_token;
            return ExtractedAccessToekn;// You can deserialize the token from here if needed
        }

        [Test]
        // Simple test method to demonstrate functionalit
        public static async Task TestTokenRetrievalAsync()
        {
            var tokenClient = new TokenClient();
            await tokenClient.GetBearerTokenAsync();
        }


    }

    public class OAuthTokenResponse
    {
        [JsonPropertyName("token_type")]
        public string token_type { get; set; }

        [JsonPropertyName("expires_in")]
        public int expires_in { get; set; }

        [JsonPropertyName("ext_expires_in")]
        public int ext_expires_in { get; set; }

        [JsonPropertyName("access_token")]
        public string access_token { get; set; }
    }
}
