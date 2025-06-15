// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using Authen.Models;
using AuthenApi.Common;
using AuthenApi.Dtos.Grant;
using Duende.IdentityServer.EntityFramework.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthenApi.Repositories.Interfaces
{
    public interface IClaimsPoliciesRepository
    {
        Task<ClientsPoliciesDto> GetClientPoliciesAsync(string search, int page = 1, int pageSize = 10);

 
        Task<int> SaveAllChangesAsync();
        Task<bool> CanInsertClaimsPoliciesAsync(ClientClaimPolicy resource);
        Task<int> AddClaimsPoliciesAsync(ClientClaimPolicy resource);
        Task<ClientClaimPolicy> ClaimsPoliciesById(int identityResourcePropertiesDto);
        Task<int> UpdateIdentityResourceAsync(ClientClaimPolicy resource);
        Task<int> DeleteEntity(ClientClaimPolicy clientEntity);

        bool AutoSaveChanges { get; set; }
    }
}