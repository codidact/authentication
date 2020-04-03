﻿using Codidact.Authentication.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Codidact.Authentication.Application.Common.Interfaces
{
    /// <summary>
    /// A Service that connects to the Core Codidact API.
    /// </summary>
    public interface ICoreApiService
    {
        Task<EntityResult> CreateMemberAsync(string url, string displayName, long userId);
    }
}
