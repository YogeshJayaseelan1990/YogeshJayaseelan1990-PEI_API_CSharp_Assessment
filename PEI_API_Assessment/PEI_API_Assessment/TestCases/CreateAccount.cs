using System;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;

namespace PEI_API_Assesment
{
    public class AccountCreator : TokenClient
    {
      
        public async Task<string> CreateAccountAsync()
        {

            var options = new RestClientOptions("https://org10da1532.crm8.dynamics.com")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/api/data/v9.2/accounts?$select=name,creditonhold,address1_latitude,description,revenue,accountcategorycode,createdon", Method.Post);
            string AccessToken = await GetBearerTokenAsync();
            request.AddHeader("Prefer", "return=representation");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer " + AccessToken); 
          
            var body = @"{
" + "\n" +
            @"    ""name"": ""Yogesh Account"",
" + "\n" +
            @"    ""creditonhold"": false,
" + "\n" +
            @"    ""address1_latitude"": 47.569583,
" + "\n" +
            @"    ""description"": ""This is the new account for Yogesh for the Assesment"",
" + "\n" +
            @"    ""revenue"": 6000000,
" + "\n" +
            @"    ""accountcategorycode"": 1
" + "\n" +
            @"}";
            request.AddStringBody(body, DataFormat.Json);
            RestResponse response = await client.ExecuteAsync(request);
            Console.WriteLine(response.Content);

            var account = JsonConvert.DeserializeObject<AccountResponse>(response.Content);

            Console.WriteLine("Account ID: " + account.accountid);

            return account.accountid;
        }

        [Test]
        // Simple test method to demonstrate functionality
        public static async Task TestCreateAccount()
        {
            var createAccount = new AccountCreator();
            await createAccount.CreateAccountAsync();
        }
    }


    public class AccountResponse
    {
        public string accountid { get; set; }
        public string name { get; set; }
        public bool creditonhold { get; set; }
        public double address1_latitude { get; set; }
        public string description { get; set; }
        public decimal revenue { get; set; }
        public int accountcategorycode { get; set; }
        public DateTime createdon { get; set; }
    }
}
