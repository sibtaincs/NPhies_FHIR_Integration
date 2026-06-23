using Microsoft.Extensions.Configuration;
using Nphies.Core.Models.Configurations;
using System;
using System.Net.Http;

namespace Nphies.Core.Brokers.Apis
{
    public partial class NphiesApiBroker : INphiesApiBroker
    {
        private readonly IConfiguration configuration;
        private readonly HttpClient apiClient;
        public NphiesApiBroker(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.apiClient = GetHttpClient(this.configuration);
        }

        private static HttpClient GetHttpClient(IConfiguration configuration)
        {
            LocalConfiguration localConfigurations =
               configuration.Get<LocalConfiguration>();

            string coreProfileBaseUrl =
                localConfigurations.NphiesApiUrl;

            return new HttpClient()
            {
                BaseAddress = new Uri(coreProfileBaseUrl)
            };

        }
    }
}
