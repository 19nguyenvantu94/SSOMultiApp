﻿// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using System.Threading.Tasks;
using AuthenApi.Dtos.Configuration;

namespace AuthenApi.Services.Interfaces
{
    public interface IIdentityResourceService
    {
        Task<IdentityResourcesDto> GetIdentityResourcesAsync(string search, int page = 1, int pageSize = 10);

        Task<IdentityResourceDto> GetIdentityResourceAsync(int identityResourceId);

        Task<bool> CanInsertIdentityResourceAsync(IdentityResourceDto identityResource);

        Task<int> AddIdentityResourceAsync(IdentityResourceDto identityResource);

        Task<int> UpdateIdentityResourceAsync(IdentityResourceDto identityResource);

        Task<int> DeleteIdentityResourceAsync(IdentityResourceDto identityResource);

        IdentityResourceDto BuildIdentityResourceViewModel(IdentityResourceDto identityResource);

        Task<IdentityResourcePropertiesDto> GetIdentityResourcePropertiesAsync(int identityResourceId, int page = 1,
            int pageSize = 10);

        Task<IdentityResourcePropertiesDto> GetIdentityResourcePropertyAsync(int identityResourcePropertyId);

        Task<int> AddIdentityResourcePropertyAsync(IdentityResourcePropertiesDto identityResourceProperties);

        Task<int> DeleteIdentityResourcePropertyAsync(IdentityResourcePropertiesDto identityResourceProperty);

        Task<bool> CanInsertIdentityResourcePropertyAsync(IdentityResourcePropertiesDto identityResourcePropertiesDto);
    }
}