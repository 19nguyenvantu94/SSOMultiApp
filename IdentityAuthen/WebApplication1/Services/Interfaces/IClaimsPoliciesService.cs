// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using Authen.Models;
using AuthenApi.Common;
using AuthenApi.Dtos.Configuration;
using AuthenApi.Dtos.Grant;
using System.Threading.Tasks;

namespace AuthenApi.Services.Interfaces
{
    public interface IClaimsPoliciesService
    {
        ClientsIdDto BuildIdentityResourceViewModel(ClientsIdDto identityResource);
        Task< ClientsPoliciesDto> GetClientPoliciesAsync(string search, int page = 1, int pageSize = 10);

        Task<int> AddClaimsPolicies(ClientsIdDto identityResource);
        Task<ClientsIdDto> GetClaimsPolicies(int identityResourceId);
        Task<int> UpdateClaimsPolicies(ClientsIdDto identityResource);
        Task<int> ClientPolicyDelete(int id);

        Task<int> ClientPolicyRoleDelete(int roleId);
    }
}