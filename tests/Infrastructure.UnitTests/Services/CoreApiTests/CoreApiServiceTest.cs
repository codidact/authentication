using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

using Codidact.Authentication.Infrastructure.Services.CoreApi;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Infrastructure.UnitTests.Services.CoreApiTests
{
    public class CoreApiServiceTest
    {
        [Theory]
        [InlineData("http://localhost:8000")]
        public async Task CreateMemberApiReturnsResult(string url)
        {
            var coreApiService = new CoreApiService(NullLogger<CoreApiService>.Instance);

            var result = await coreApiService.CreateMemberAsync(url, "John", 1);

            Assert.True(result.Success);
        }
    }
}
