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
        public void InitRestClient()
        {
            this.serviceUrl = "http://localhost:52157/api/shopping-list";
            this.client = new RestClient(this.serviceUrl);          
            this.client.AddDefaultHeader("Authorization", Guid.NewGuid().ToString("N"));

            this.client.Get(new RestRequest("/")).ResponseStatus.Should().NotBe(ResponseStatus.Error, $"The service should be running on url {this.serviceUrl}. Please run the web application before executing the integration tests.");

            // TODO: Use kestrel here to start an instance instead of asking the app to be run manually.
        }

        [ClassInitialize]
        public static void ChecksServerIsRunning(TestContext context)
        {
            // TODO: Use kestrel here to start an instance instead of asking the app to be run manually.
            var baseUrl = "http://localhost:52157/api/shopping-list";
            new RestClient(baseUrl)
                .Get(new RestRequest("/"))
                .ResponseStatus.Should().NotBe(ResponseStatus.Error,
                $"The service should be running on url {baseUrl}. Please run the web application before executing the integration tests.");

        }

        [TestMethod]
        public void Get_request_on_ShoppingList_resource()
        {
            var restRequest = new RestRequest("/drinks");

            var response = this.client.Get(restRequest);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [TestMethod]
        public void AccessApiWithUnauthorizedApiKey()
        {
            var restRequest = new RestRequest("/drinks");
            restRequest.AddHeader("Authorization", "Bearer JustARegularTokenThatDoesntLookLikeAnApiKey");

            var response = this.client.Get(restRequest);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [TestMethod]
        public void AccessApiWithoutApiKey()
        {
            var restRequest = new RestRequest("/drinks");
            var client = new RestClient(this.serviceUrl);
            var response = client.Get(restRequest);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
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
        public void Post_request_on_ShoppingList_with_incomplete_payload()
        {            
            // The specs ask us to adding drinks here, not create a new resource. it is not truly a REST api, since "add" is an action and not the location of a resource.
            var restRequest = new RestRequest("/add-drink");

            var payload = "{\"name\":\"Pepsi\"}";
            restRequest.AddParameter("application/json", payload, ParameterType.RequestBody);
            var response = this.client.Post(restRequest);
            
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);                        
        }

        [TestMethod]
        public void Post_request_on_ShoppingList_with_bad_format_payload()
        {            
            // The specs ask us to adding drinks here, not create a new resource. it is not truly a REST api, since "add" is an action and not the location of a resource.
            var restRequest = new RestRequest("/add-drink");

            var payload = "{\"name\":\"Pepsi\",\"quantity\":\"one\"}";
            restRequest.AddParameter("application/json", payload, ParameterType.RequestBody);
            var response = this.client.Post(restRequest);
            
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);                        
        }

        [TestMethod]
        public void Post_request_on_ShoppingList_resource_for_2_new_drinks_followed_by_Get_ShoppingList()
        {
            var restRequest = new RestRequest("/add-drink");

            var newDrink = Guid.NewGuid().ToString("N");            
            string payload = $"{{\"name\":\"{newDrink}\",\"quantity\":2}}";

            restRequest.AddParameter("application/json", payload, ParameterType.RequestBody);

            this.client.Post(restRequest);

            var request = new RestRequest("/drinks");
            var response = this.client.Get(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var jsonResponse = JArray.Parse(response.Content);
            var newDrinkToken = jsonResponse.Single(token => token["name"].Value<string>().Equals(newDrink, StringComparison.OrdinalIgnoreCase));
            newDrinkToken["quantity"].Value<int>().Should().Be(2);
        }

        [TestMethod]
        public void two_Post_requests_on_ShoppingList_resource_for_2_new_drinks_each_followed_by_Get_ShoppingList()
        {
            var restRequest = new RestRequest("/add-drink");

            var newDrink = Guid.NewGuid().ToString("N");
            string payload = $"{{\"name\":\"{newDrink}\",\"quantity\":2}}";

            restRequest.AddParameter("application/json", payload, ParameterType.RequestBody);

            this.client.Post(restRequest);

            this.client.Post(restRequest);

            var request = new RestRequest("/drinks");
            var response = this.client.Get(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var jsonResponse = JArray.Parse(response.Content);
            var newDrinkToken = jsonResponse.Single(token => token["name"].Value<string>().Equals(newDrink, StringComparison.OrdinalIgnoreCase));
            newDrinkToken["quantity"].Value<int>().Should().Be(4);
        }

        [TestMethod]
        public void Get_request_on_ShoppingList_for_unknown_drink()
        {
            var unknownDrink = Guid.NewGuid().ToString("N");
            var restRequest = new RestRequest($"/drinks/{unknownDrink}");
           
            var response = this.client.Get(restRequest);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void Get_request_on_ShoppingList_for_known_drink()
        {

            var restRequest = new RestRequest("/add-drink");

            var newDrink = Guid.NewGuid().ToString("N");
            string payload = $"{{\"name\":\"{newDrink}\",\"quantity\":2}}";

            restRequest.AddParameter("application/json", payload, ParameterType.RequestBody);

            this.client.Post(restRequest);

            var getRequest = new RestRequest($"/drinks/{newDrink}");

            var response = this.client.Get(getRequest);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            JObject.Parse(response.Content)["quantity"].Value<int>().Should().Be(2);
        }

        [TestMethod]
        public void Delete_request_on_ShoppingList_for_known_drink()
        {

            var restRequest = new RestRequest("/add-drink");

            var newDrink = Guid.NewGuid().ToString("N");
            string payload = $"{{\"name\":\"{newDrink}\",\"quantity\":2}}";

            restRequest.AddParameter("application/json", payload, ParameterType.RequestBody);

            this.client.Post(restRequest);

            var getRequest = new RestRequest($"/drinks/{newDrink}");

            var response = this.client.Get(getRequest);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            JObject.Parse(response.Content)["quantity"].Value<int>().Should().Be(2);

            var deleteRequest = new RestRequest($"/drinks/{newDrink}");
            var deleteResponse = this.client.Delete(deleteRequest);
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseAfterDelete = this.client.Get(getRequest);
            responseAfterDelete.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }


        [TestMethod]
        public void Delete_request_on_ShoppingList_for_unknown_drink()
        {
            var unknownDrink = Guid.NewGuid().ToString("N");

            var deleteRequest = new RestRequest($"/drinks/{unknownDrink}");
            var deleteResponse = this.client.Delete(deleteRequest);
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void Put_on_known_drink_updates_quantity_accordingly_and_returns_modified_drink_in_body()
        {
            var restRequest = new RestRequest("/add-drink");

            var newDrink = Guid.NewGuid().ToString("N");
            string payload = $"{{\"name\":\"{newDrink}\",\"quantity\":2}}";
            restRequest.AddParameter("application/json", payload, ParameterType.RequestBody);
            this.client.Post(restRequest);

            var request = new RestRequest($"/drinks/{newDrink}");
            string payloadUpdate = $"{{\"name\":\"{newDrink}\",\"quantity\":1337}}";
            request.AddParameter("application/json", payloadUpdate, ParameterType.RequestBody);
            var response = this.client.Put(request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Should().NotBeNullOrWhiteSpace();
            JObject.Parse(response.Content)["quantity"].Value<int>().Should().Be(1337);

            var getRequest = new RestRequest($"/drinks/{newDrink}");

            var getResponse = this.client.Get(getRequest);
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            JObject.Parse(getResponse.Content)["quantity"].Value<int>().Should().Be(1337);
        }

        [TestMethod]
        public void Put_on_unknown_drink_returns_NotFound()
        {
            var unknownDrink = Guid.NewGuid().ToString("N");

            var request = new RestRequest($"/drinks/{unknownDrink}");
            string payloadUpdate = $"{{\"quantity\":1337}}";
            request.AddParameter("application/json", payloadUpdate, ParameterType.RequestBody);
            var response = this.client.Put(request);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);        
        }

        [TestMethod]
        public void Put_on_drink_with_invalid_payload()
        {
            string drinkName = Guid.NewGuid().ToString("N");
            var request = new RestRequest($"/drinks/{drinkName}");
            string payloadUpdate = $"{{\"count\":1337}}";
            request.AddParameter("application/json", payloadUpdate, ParameterType.RequestBody);
            var response = this.client.Put(request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);        
        }

        [TestMethod]
        public void Put_on_drink_without_a_payload()
        {
            string drinkName = Guid.NewGuid().ToString("N");
            var request = new RestRequest($"/drinks/{drinkName}");
            string payloadUpdate = $"  ";
            request.AddParameter("application/json", payloadUpdate, ParameterType.RequestBody);
            var response = this.client.Put(request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);        
        }

        [TestMethod]
        public void Post_request_on_ShoppingList_without_payload()
        {
            // The specs ask us to adding drinks here, not create a new resource. it is not truly a REST api, since "add" is an action and not the location of a resource.
            var restRequest = new RestRequest("/add-drink");

            var payload = " ";
            restRequest.AddParameter("application/json", payload, ParameterType.RequestBody);
            var response = this.client.Post(restRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }


        [TestMethod]
        public void Delete_request_on_ShoppingList_without_passing_drinkName_returns_NotFound()
        {
            var unknownDrink = string.Empty;

            var deleteRequest = new RestRequest($"/drinks/{unknownDrink}");
            var deleteResponse = this.client.Delete(deleteRequest);
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
