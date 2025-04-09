using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace PEI_API_Assesment
{
    public class GetAccount : TokenClient
    {
        private const string BaseUrl = "https://org10da1532.crm8.dynamics.com";
      

        [Test]
        public async Task TestGetAccountAsync()
        {
            var options = new RestClientOptions(BaseUrl)
            {
                MaxTimeout = -1
            };
            string AccessToken = await GetBearerTokenAsync();
            var client = new RestClient(options);
            AccountCreator accCreate = new AccountCreator();
            string AccountID = await accCreate.CreateAccountAsync();
            // Replace with a valid Account GUID from your environment
            var request = new RestRequest("/api/data/v9.2/accounts("+ AccountID+")", Method.Get);

            // Set Authorization
            request.AddHeader("Authorization", "Bearer " + AccessToken);

            
            // Execute
            RestResponse response = await client.ExecuteAsync(request);

            // Output response
            Console.WriteLine("Status: " + response.StatusCode);
            Console.WriteLine("Content: " + response.Content);
            Assert.AreEqual(200, (int) response.StatusCode);

            GetAccountResponse account = JsonConvert.DeserializeObject<GetAccountResponse>(response.Content);

            Console.WriteLine($"Account ID: {account.accountid}");

            Assert.AreEqual(AccountID, account.accountid);
           

        }


    }

    public class GetAccountResponse
    {
        public string accountid { get; set; }
    }
}
