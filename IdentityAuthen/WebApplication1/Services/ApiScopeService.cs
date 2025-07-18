﻿// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using System.Collections.Generic;
using System.Threading.Tasks;
using Duende.IdentityServer.EntityFramework.Entities;
using AuthenApi.Dtos.Configuration;
using AuthenApi.Helpers;
using AuthenApi.Mappers;
using AuthenApi.Resources;
using AuthenApi.Services.Interfaces;
using AuthenApi.Common;
using AuthenApi.Repositories.Interfaces;
using AuthenApi.ExceptionHandling;
using Authen.Resources;

namespace AuthenApi.Services
{
    public class ApiScopeService : IApiScopeService
    {
        protected readonly IApiScopeRepository ApiScopeRepository;
        protected readonly IApiScopeServiceResources ApiScopeServiceResources;
        //protected readonly IAuditEventLogger AuditEventLogger;

        public ApiScopeService(IApiScopeServiceResources apiScopeServiceResources, IApiScopeRepository apiScopeRepository
            //, IAuditEventLogger auditEventLogger
            )
        {
            ApiScopeRepository = apiScopeRepository;
            //AuditEventLogger = auditEventLogger;
            ApiScopeServiceResources = apiScopeServiceResources;

        }

        public virtual async Task<ApiScopesDto> GetApiScopesAsync(string search, int page = 1, int pageSize = 10)
        {
            var pagedList = await ApiScopeRepository.GetApiScopesAsync(search, page, pageSize);

            var apiScopesDto = pagedList.ToModel();

            //await AuditEventLogger.LogEventAsync(new ApiScopesRequestedEvent(apiScopesDto));

            return apiScopesDto;
        }

        public virtual async Task<ICollection<string>> GetApiScopesNameAsync(string scope, int limit = 0)
        {
            var scopes = await ApiScopeRepository.GetApiScopesNameAsync(scope, limit);

            return scopes;
        }

        public virtual async Task<ApiScopePropertiesDto> GetApiScopePropertiesAsync(int apiScopeId, int page = 1, int pageSize = 10)
        {
            var apiScope  = await ApiScopeRepository.GetApiScopeAsync(apiScopeId);
            if (apiScope == null)
                throw new UserFriendlyErrorPageException(string.Format(ApiScopeServiceResources.ApiScopeDoesNotExist().Description, apiScopeId), ApiScopeServiceResources.ApiScopeDoesNotExist().Description);

            PagedList<ApiScopeProperty> pagedList = await ApiScopeRepository.GetApiScopePropertiesAsync(apiScopeId, page, pageSize);
            var apiScopePropertiesDto = pagedList.ToModel();
            apiScopePropertiesDto.ApiScopeId = apiScopeId;
            apiScopePropertiesDto.ApiScopeName = await ApiScopeRepository.GetApiScopeNameAsync(apiScopeId);

            //await AuditEventLogger.LogEventAsync(new ApiScopePropertiesRequestedEvent(apiScopeId, apiScopePropertiesDto));

            return apiScopePropertiesDto;
        }

        public virtual async Task<ApiScopeDto> GetApiScopeAsync(int apiScopeId)
        {
            var apiScope = await ApiScopeRepository.GetApiScopeAsync(apiScopeId);
            if (apiScope == null) throw new UserFriendlyErrorPageException(string.Format(ApiScopeServiceResources.ApiScopeDoesNotExist().Description, apiScopeId), ApiScopeServiceResources.ApiScopeDoesNotExist().Description);

            var apiScopeDto = apiScope.ToModel();

            //await AuditEventLogger.LogEventAsync(new ApiScopeRequestedEvent(apiScopeDto));

            return apiScopeDto;
        }

        public virtual async Task<int> AddApiScopeAsync(ApiScopeDto apiScope)
        {
            var canInsert = await CanInsertApiScopeAsync(apiScope);
            if (!canInsert)
            {
                throw new UserFriendlyViewException(string.Format(ApiScopeServiceResources.ApiScopeExistsValue().Description, apiScope.Name), ApiScopeServiceResources.ApiScopeExistsKey().Description, apiScope);
            }

            var scope = apiScope.ToEntity();

            var added = await ApiScopeRepository.AddApiScopeAsync(scope);

            //await AuditEventLogger.LogEventAsync(new ApiScopeAddedEvent(apiScope));

            return added;
        }

        public virtual ApiScopeDto BuildApiScopeViewModel(ApiScopeDto apiScope)
        {
            ComboBoxHelpers.PopulateValuesToList(apiScope.UserClaimsItems, apiScope.UserClaims);

            return apiScope;
        }

        public virtual async Task<int> UpdateApiScopeAsync(ApiScopeDto apiScope)
        {
            var canInsert = await CanInsertApiScopeAsync(apiScope);
            if (!canInsert)
            {
                throw new UserFriendlyViewException(string.Format(ApiScopeServiceResources.ApiScopeExistsValue().Description, apiScope.Name), ApiScopeServiceResources.ApiScopeExistsKey().Description, apiScope);
            }

            var scope = apiScope.ToEntity();

            var originalApiScope = await GetApiScopeAsync(apiScope.Id);

            var updated = await ApiScopeRepository.UpdateApiScopeAsync(scope);

            //await AuditEventLogger.LogEventAsync(new ApiScopeUpdatedEvent(originalApiScope, apiScope));

            return updated;
        }

        public virtual async Task<int> DeleteApiScopeAsync(ApiScopeDto apiScope)
        {
            var scope = apiScope.ToEntity();

            var deleted = await ApiScopeRepository.DeleteApiScopeAsync(scope);

            //await AuditEventLogger.LogEventAsync(new ApiScopeDeletedEvent(apiScope));

            return deleted;
        }

        public virtual async Task<bool> CanInsertApiScopeAsync(ApiScopeDto apiScopeDto)
        {
            var apiScope = apiScopeDto.ToEntity();

            return await ApiScopeRepository.CanInsertApiScopeAsync(apiScope);
        }

        public virtual async Task<ApiScopePropertiesDto> GetApiScopePropertyAsync(int apiScopePropertyId)
        {
            var apiScopeProperty = await ApiScopeRepository.GetApiScopePropertyAsync(apiScopePropertyId);
            if (apiScopeProperty == null) throw new UserFriendlyErrorPageException(string.Format(ApiScopeServiceResources.ApiScopePropertyDoesNotExist().Description, apiScopePropertyId));

            var apiScopePropertiesDto = apiScopeProperty.ToModel();
            apiScopePropertiesDto.ApiScopeId = apiScopeProperty.ScopeId;
            apiScopePropertiesDto.ApiScopeName = await ApiScopeRepository.GetApiScopeNameAsync(apiScopeProperty.ScopeId);

            //await AuditEventLogger.LogEventAsync(new ApiScopePropertyRequestedEvent(apiScopePropertyId, apiScopePropertiesDto));

            return apiScopePropertiesDto;
        }

        private async Task BuildApiScopePropertiesViewModelAsync(ApiScopePropertiesDto apiScopeProperties)
        {
            var apiResourcePropertiesDto = await GetApiScopePropertiesAsync(apiScopeProperties.ApiScopeId);
            apiScopeProperties.ApiScopeProperties.AddRange(apiResourcePropertiesDto.ApiScopeProperties);
            apiScopeProperties.TotalCount = apiResourcePropertiesDto.TotalCount;
        }

        public virtual async Task<int> AddApiScopePropertyAsync(ApiScopePropertiesDto apiScopeProperties)
        {
            var canInsert = await CanInsertApiScopePropertyAsync(apiScopeProperties);
            if (!canInsert)
            {
                await BuildApiScopePropertiesViewModelAsync(apiScopeProperties);
                throw new UserFriendlyViewException(string.Format(ApiScopeServiceResources.ApiScopePropertyExistsValue().Description, apiScopeProperties.Key), ApiScopeServiceResources.ApiScopePropertyExistsKey().Description, apiScopeProperties);
            }

            var apiScopeProperty = apiScopeProperties.ToEntity();

            var saved = await ApiScopeRepository.AddApiScopePropertyAsync(apiScopeProperties.ApiScopeId, apiScopeProperty);

            //await AuditEventLogger.LogEventAsync(new ApiScopePropertyAddedEvent(apiScopeProperties));

            return saved;
        }

        public virtual async Task<int> DeleteApiScopePropertyAsync(ApiScopePropertiesDto apiScopeProperty)
        {
            var propertyEntity = apiScopeProperty.ToEntity();

            var deleted = await ApiScopeRepository.DeleteApiScopePropertyAsync(propertyEntity);

            //await AuditEventLogger.LogEventAsync(new ApiScopePropertyDeletedEvent(apiScopeProperty));

            return deleted;
        }

        public virtual async Task<bool> CanInsertApiScopePropertyAsync(ApiScopePropertiesDto apiResourceProperty)
        {
            var resource = apiResourceProperty.ToEntity();

            return await ApiScopeRepository.CanInsertApiScopePropertyAsync(resource);
        }
    }
}
