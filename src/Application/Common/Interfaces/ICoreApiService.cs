using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Codidact.Authentication.Application.Common.Interfaces
{
    /// <summary>
    /// Service that connect to the Core Codidact API
    /// </summary>
    public interface ICoreApiService
    {
        Task<bool> CreateMember(string displayName, long userId);
    }
}
