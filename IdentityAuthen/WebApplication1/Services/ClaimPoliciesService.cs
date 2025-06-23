// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using Authen.Models;
using AuthenApi.Common;
using AuthenApi.Dtos.Configuration;
using AuthenApi.Dtos.Enums;
using AuthenApi.Dtos.Grant;
using AuthenApi.ExceptionHandling;
using AuthenApi.Helpers;
using AuthenApi.Mappers;
using AuthenApi.Repositories;
using AuthenApi.Repositories.Interfaces;
using AuthenApi.Resources;
using AuthenApi.Services.Interfaces;
using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenApi.Services
{
    public class ClaimPoliciesService : IClaimsPoliciesService
    {
        protected readonly IClaimsPoliciesRepository ClientRepository;
        //protected readonly IAuditEventLogger AuditEventLogger;
        private const string SharedSecret = "SharedSecret";

        protected readonly IClaimsPoliciesServiceResources IClaimsPoliciesServiceResources;


        public ClaimPoliciesService(IClaimsPoliciesRepository clientRepository,

                IClaimsPoliciesServiceResources claimsPoliciesServiceResources
            //, IAuditEventLogger auditEventLogger
            )
        {
            ClientRepository = clientRepository;
            IClaimsPoliciesServiceResources = claimsPoliciesServiceResources;
            //AuditEventLogger = auditEventLogger;
        }

        public async Task<ClientsPoliciesDto> GetClientPoliciesAsync(string search, int page = 1, int pageSize = 10)
        {
            return await ClientRepository.GetClientPoliciesAsync(search, page, pageSize);
        }

        public ClientClaimPolicy BuildIdentityResourceViewModel(ClientClaimPolicy identityResource)
        {

            //ComboBoxHelpers.PopulateValuesToList(identityResource.UserClaimsItems, identityResource.UserClaims);

            return identityResource;

        }

        public async Task<int> AddClaimsPolicies(ClientClaimPolicy identityResource)
        {
            var canInsert = await CanInsertClaimsPolicies(identityResource);
            if (!canInsert)
            {
                throw new UserFriendlyViewException(string.Format(IClaimsPoliciesServiceResources.ClaimsPoliciesExistsKey().Description, identityResource.Client.ClientId), IClaimsPoliciesServiceResources.ClaimsPoliciesExistsKey().Description, identityResource);
            }

            var saved = await ClientRepository.AddClaimsPoliciesAsync(identityResource);

            //await AuditEventLogger.LogEventAsync(new IdentityResourceAddedEvent(identityResource));

            return saved;
        }

        public virtual async Task<bool> CanInsertClaimsPolicies(ClientClaimPolicy identityResourcePropertiesDto)
        {

            return await ClientRepository.CanInsertClaimsPoliciesAsync(identityResourcePropertiesDto);
        }



        public async Task<ClientClaimPolicy> GetClaimsPolicies(int identityResourceId)
        {
            return await ClientRepository.ClaimsPoliciesById(identityResourceId);
        }

        //public Task UpdateClaimsPolicies(ClientClaimPolicy identityResource)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<int> UpdateClaimsPolicies(ClientClaimPolicy identityResource)
        {
            var canInsert = await CanInsertClaimsPolicies(identityResource);
            if (!canInsert)
            {
                throw new UserFriendlyViewException(string.Format(IClaimsPoliciesServiceResources.ClaimsPoliciesExistsValue().Description, identityResource.Client.ClientId), IClaimsPoliciesServiceResources.ClaimsPoliciesExistsValue().Description, identityResource);
            }

            var updated = await ClientRepository.UpdateIdentityResourceAsync(identityResource);

            //await AuditEventLogger.LogEventAsync(new IdentityResourceUpdatedEvent(originalIdentityResource, identityResource));

            return updated;
        }

        public async Task<int> ClientPolicyDelete(int id)
        {
            var clientEntity = await ClientRepository.ClaimsPoliciesById(id);

            if (clientEntity == null)
            {
                throw new UserFriendlyViewException(IClaimsPoliciesServiceResources.ClaimsPoliciesDoesNotExist().Description, IClaimsPoliciesServiceResources.ClaimsPoliciesDoesNotExist().Description, id);
            }

            return await ClientRepository.DeleteEntity(clientEntity);

        }
    }
}