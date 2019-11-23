﻿namespace CodeGolf.Integration.Test.Pages
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using CodeGolf.Integration.Test.Fixtures;
    using CodeGolf.Integration.Test.Tooling;
    using FluentAssertions;
    using Xunit;

    public class DashboardPageShould : IClassFixture<CustomWebApplicationFactory<CodeGolf.Web.Startup>>
    {
        private readonly HttpClient client;

        public DashboardPageShould(CustomWebApplicationFactory<CodeGolf.Web.Startup> client)
        {
            this.client = client.CreateClient();
        }

        [Fact(Skip = "Need to fix auth mock")]
        public async Task GetDashboard()
        {
            this.client.DefaultRequestHeaders.Add(AuthenticatedTestRequestMiddleware.TestingHeader, AuthenticatedTestRequestMiddleware.TestingHeaderValue);
            this.client.DefaultRequestHeaders.Add("Authorization", "Bearer Admin");
            var response = await this.client.GetAsync("/dashboard");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task RequireAuth()
        {
            var response = await this.client.GetAsync("/dashboard");
            response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        }
    }
}