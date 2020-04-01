using System.Threading.Tasks;

namespace Codidact.Authentication.Application.Common.Interfaces
{
    /// <summary>
    /// Provides access to secret configuration options like connection strings
    /// and api keys.
    /// <summary>
    public interface ISecretsService
    {
        Task<string> Get(string key);
    }
}
