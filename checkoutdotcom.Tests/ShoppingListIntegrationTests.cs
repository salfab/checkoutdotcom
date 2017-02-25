using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            this.serviceUrl = "http://localhost:52157/api";
            this.client = new RestClient(this.serviceUrl);
            this.client.Get(new RestRequest("/shopping-list", Method.GET)).ResponseStatus.Should().NotBe(ResponseStatus.Error, $"The service should be running on url {this.serviceUrl}. Please run the web application before executing the integration tests.");

            // TODO: Use kestrel here to start an instance instead of asking the app to be run manually.
        }

        [TestMethod]
        public void Get_request_on_ShoppingList_resource()
        {
            var response = this.client.Get(new RestRequest("/", Method.GET));
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [TestMethod]
        public void Post_request_on_ShoppingList_resource_for_1_Pepsi()
        {            
            // The specs ask us to adding drinks here, not create a new resource. it is not truly a REST api, since "add" is an action and not the location of a resource.
            var response = this.client.Get(new RestRequest("/add-drink", Method.POST));
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
