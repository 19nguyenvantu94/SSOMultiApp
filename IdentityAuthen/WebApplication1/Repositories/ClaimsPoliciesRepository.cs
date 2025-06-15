using Authen.Data;
using Authen.Models;
using AuthenApi.Common;
using AuthenApi.Dtos.Enums;
using AuthenApi.Dtos.Grant;
using AuthenApi.Helpers;
using AuthenApi.Repositories.Interfaces;
using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.Models;
using OpenTelemetry.Resources;
using System.Linq.Expressions;

namespace Authen.Repositories
{
    public class ClaimsPoliciesRepository : IClaimsPoliciesRepository
    {
        protected readonly ApplicationDbContext DbContext;
        public bool AutoSaveChanges { get; set; } = true;

        public ClaimsPoliciesRepository(ApplicationDbContext _dbContext)
        {
            DbContext = _dbContext;
        }

        public async Task<ClientsPoliciesDto> GetClientPoliciesAsync(string search, int page, int pageSize)
        {
            var pagedList = new ClientsPoliciesDto();

            Expression<Func<ClientClaimPolicy, bool>> searchCondition = x => x.ClientId.Contains(search);
            var clients = await DbContext.ClientClaimPolicies
                .WhereIf(!string.IsNullOrEmpty(search), searchCondition)
                .PageBy(x => x.Id, page, pageSize)
                .ToListAsync();

            //// Map ClientClaimPolicy to ClientsPoliciesDto
            //var clientPoliciesDtos = clients.Select(client => new ClientsPoliciesDto
            //{
            //    TotalCount = 0, // Placeholder, will be set later
            //    PageSize = pageSize,
            //    ClientsPolicies = new List<ClientClaimPolicy> { client }
            //}).ToList();

            pagedList.ClientsPolicies.AddRange(clients);
            pagedList.TotalCount = await DbContext.ClientClaimPolicies
                .WhereIf(!string.IsNullOrEmpty(search), searchCondition)
                .CountAsync();
            pagedList.PageSize = pageSize;

            return pagedList;
        }

        public async Task<int> SaveAllChangesAsync()
        {
            return await DbContext.SaveChangesAsync();
        }

        public async Task<bool> CanInsertClaimsPoliciesAsync(ClientClaimPolicy resource)
        {

            if (resource.Id == 0 )
            {
                var existsWithClientName = await DbContext.Clients.Where(x => x.ClientId == resource.ClientId).SingleOrDefaultAsync();
                return existsWithClientName == null;
            }
            else
            {
                var existsWithClientName = await DbContext.Clients.Where(x => x.ClientId == resource.ClientId && x.Id != resource.Id).SingleOrDefaultAsync();
                return existsWithClientName == null;
            }
         }

        public async Task<int> AddClaimsPoliciesAsync(ClientClaimPolicy resource)
        {
            await DbContext.ClientClaimPolicies.AddAsync(resource);

            return await AutoSaveChangesAsync();
        }

        //public   Task<IdentityResourceProperty> GetIdentityResourcePropertyAsync(int identityResourcePropertyId)
        //{
        //    return DbContext.IdentityResourceProperties
        //        .Include(x => x.IdentityResource)
        //        .Where(x => x.Id == identityResourcePropertyId)
        //        .SingleOrDefaultAsync();
        //}

        public async Task<ClientClaimPolicy> ClaimsPoliciesById(int identityResourcePropertiesDto)
        {
            return await DbContext.ClientClaimPolicies
                 .Where(x => x.Id == identityResourcePropertiesDto)
                .SingleOrDefaultAsync();
        }

        public virtual async Task<int> UpdateIdentityResourceAsync(ClientClaimPolicy identityResource)
        {

            //Update with new data
            DbContext.ClientClaimPolicies.Update(identityResource);

            return await AutoSaveChangesAsync();
        }

        protected virtual async Task<int> AutoSaveChangesAsync()
        {
            return AutoSaveChanges ? await DbContext.SaveChangesAsync() : (int)SavedStatus.WillBeSavedExplicitly;
        }

        public virtual async Task<int> DeleteEntity(ClientClaimPolicy clientEntity)
        {
            DbContext.ClientClaimPolicies.Remove(clientEntity);

            return await AutoSaveChangesAsync();
        }
    }
}
