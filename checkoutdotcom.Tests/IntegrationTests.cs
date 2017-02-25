using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace checkoutdotcom.Tests
{
    using System.Net;

    using FluentAssertions;

    using RestSharp;

    [TestClass]
    public class IntegrationTests
    {
        private string serviceUrl;

        [TestInitialize]
        public void InitTestServer()
        {
            this.serviceUrl = "http://localhost:52157";
            var client = new RestClient(this.serviceUrl);
            client.Get(new RestRequest("/")).ResponseStatus.Should().NotBe(ResponseStatus.Error, $"The service should be running on url {this.serviceUrl}. Please run the web application before executing the integration tests.");

            // TODO: Use kestrel here to start an instance instead of asking the app to be run manually.
        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
