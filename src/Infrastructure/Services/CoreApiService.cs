using Codidact.Authentication.Application.Common.Interfaces;
using Codidact.Authentication.Domain.Common;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Codidact.Authentication.Infrastructure.Services
{
    public class CoreApiService : ICoreApiService
    {
        private readonly ILogger<CoreApiService> _logger;

        public CoreApiService(ILogger<CoreApiService> logger)
        {
            _logger = logger;
        }

        public async Task<EntityResult> CreateMemberAsync(string url, string displayName, long userId)
        {
            _logger.LogInformation("Create Member for the user id");

            var httpClient = GetApiHttpClient(url);
            var postBody = GetRequestContent(
                new
                {
                    displayName,
                    userId
                });
            var response = await httpClient.PostAsync("member/create", postBody);
            _logger.LogInformation("Member Create Request finished");

            if (!response.IsSuccessStatusCode)
            {
                return new EntityResult(false);
            }

            var result = await GetResponseContent<EntityResult>(response);

            return result;
        }

        private HttpClient GetApiHttpClient(string url)
        {
            var uri = new Uri($"{url}/api/v1/");
            var httpClient = new HttpClient
            {
                BaseAddress = uri
            };

            return httpClient;
        }

        public StringContent GetRequestContent(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }

        public async Task<T> GetResponseContent<T>(HttpResponseMessage response)
        {
            var stringResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(stringResponse);
        }
    }
}
