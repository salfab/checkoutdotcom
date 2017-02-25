using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json.Linq;

namespace checkoutdotcom.Tests
{
    using System.Net;

    using FluentAssertions;

    using RestSharp;

    [TestClass]
    public class ShoppingListIntegrationTests
    {
        private string serviceUrl;

        private RestClient client;

        [TestInitialize]
        public void InitTestServer()
        {
            this.serviceUrl = "http://localhost:52157/api/shopping-list";
            this.client = new RestClient(this.serviceUrl);                        
            this.client.Get(new RestRequest("/")).ResponseStatus.Should().NotBe(ResponseStatus.Error, $"The service should be running on url {this.serviceUrl}. Please run the web application before executing the integration tests.");

            // TODO: Use kestrel here to start an instance instead of asking the app to be run manually.
        }

        [TestMethod]
        public void Get_request_on_ShoppingList_resource()
        {
            var restRequest = new RestRequest("/")
                                  {
                                      Method = Method.GET
                                  };

            var response = this.client.Execute(restRequest);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [TestMethod]
        public void Post_request_on_ShoppingList_resource_for_1_Pepsi()
        {            
            // The specs ask us to adding drinks here, not create a new resource. it is not truly a REST api, since "add" is an action and not the location of a resource.
            var restRequest = new RestRequest("/add-drink");

            var payload = "{\"name\":\"Pepsi\",\"quantity\":1}";
            restRequest.AddParameter("application/json", payload, ParameterType.RequestBody);
            var response = this.client.Post(restRequest);
            
            response.StatusCode.Should().Be(HttpStatusCode.OK);                        
        }

        [TestMethod]
        public void Post_request_on_ShoppingList_resource_for_2_new_drinks_followed_by_Get_ShoppingList()
        {
            var restRequest = new RestRequest("/add-drink");

            var newDrink = Guid.NewGuid().ToString("N");            
            string payload = $"{{\"name\":\"{newDrink}\",\"quantity\":2}}";

            restRequest.AddParameter("application/json", payload, ParameterType.RequestBody);

            this.client.Post(restRequest);

            var request = new RestRequest("/");
            var response = this.client.Get(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var jsonResponse = JArray.Parse(response.Content);
            var newDrinkToken = jsonResponse.Single(token => token["name"].Value<string>().Equals(newDrink, StringComparison.OrdinalIgnoreCase));
            newDrinkToken["quantity"].Value<int>().Should().Be(2);
        }
    }
}
