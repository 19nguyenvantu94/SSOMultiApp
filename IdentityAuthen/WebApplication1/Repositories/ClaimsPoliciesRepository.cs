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

            Expression<Func<ClientClaimPolicy, bool>> searchCondition = x => x.Client.ClientId.Contains(search);
            var clients = await DbContext.ClientClaimPolicies
                .WhereIf(!string.IsNullOrEmpty(search), searchCondition)
                .Include(x=>x.PolicyRoles)
                .Include(x=>x.Client)
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

        public async Task<bool> CanInsertClaimsPoliciesAsync(ClientsIdDto resource)
        {

            if (resource.Id == 0)
            {
                var existsWithClientName = await DbContext.ClientClaimPolicies.Where(x => x.IdClient == resource.ClientId).SingleOrDefaultAsync();
                return existsWithClientName == null;
            }
            else
            {
                var existsWithClientName = await DbContext.ClientClaimPolicies.Where(x => x.IdClient == resource.ClientId && x.Id != resource.Id).SingleOrDefaultAsync();
                return existsWithClientName == null;
            }
        }

        public async Task<int> AddClaimsPoliciesAsync(ClientsIdDto resource)
        {

            ClientClaimPolicy clientClaimPolicy = new ClientClaimPolicy();
            clientClaimPolicy.IdClient = resource.ClientId;

            var dataAdd = await DbContext.ClientClaimPolicies.AddAsync(clientClaimPolicy);

            await DbContext.SaveChangesAsync(); // Save to generate Id

            ClientClaimPolicyRole clientClaimPolicyRole = new ClientClaimPolicyRole
            {
                RoleId = resource.RoleId,
                ClientClaimPolicyId = dataAdd.Entity.Id,
            };


            await DbContext.ClientClaimPolicyRoles.AddAsync(clientClaimPolicyRole);

            return await AutoSaveChangesAsync();
        }

        public async Task<ClientsIdDto> ClaimsPoliciesById(int identityResourcePropertiesDto)
        {
            var data = await DbContext.ClientClaimPolicies
                 .Where(x => x.Id == identityResourcePropertiesDto)
                .FirstOrDefaultAsync();

            var getAllClients = await DbContext.ClientClaimPolicies.Select(x => x.IdClient).ToListAsync();

            var getAllRoles = await DbContext.ClientClaimPolicyRoles.Select(x => x.RoleId).ToListAsync();

            // Fix for CS7036: Ensure the SelectItem constructor is called with the required parameters 'id' and 'text'.
            ClientsIdDto clientsIdDto = new ClientsIdDto
            {
                Id = identityResourcePropertiesDto == 0 ? 0 : data!.Id,
                Client = identityResourcePropertiesDto == 0 ? null : data!.Client,
                Roles = identityResourcePropertiesDto == 0 ? null : data!.PolicyRoles.Select(role => new SelectItem(role.RoleId.ToString(), role.Role.Name)).ToList(),
                ClientsList = DbContext.Clients
                    .Where(x => !getAllClients.Contains(x.Id))
                    .Select(c => new SelectItem(c.Id.ToString(), c.ClientId))
                    .ToList(),
                RolesList = DbContext.Roles
                    .Where(x => !getAllRoles.Contains(x.Id))
                    .Select(r => new SelectItem(r.Id.ToString(), r.Name))
                    .ToList()
            };

            return clientsIdDto;
        }

        public virtual async Task<int> UpdateIdentityResourceAsync(ClientsIdDto identityResource)
        {

            //Update with new data
            //DbContext.ClientClaimPolicies.Update(identityResource);

            return await AutoSaveChangesAsync();
        }

        protected virtual async Task<int> AutoSaveChangesAsync()
        {
            return AutoSaveChanges ? await DbContext.SaveChangesAsync() : (int)SavedStatus.WillBeSavedExplicitly;
        }

        public async Task<ClientClaimPolicy?> CheckForDelete(int id)
        {
            var data = await DbContext.ClientClaimPolicies
                 .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return data;
        }

        public virtual async Task<int> DeleteEntity(ClientClaimPolicy clientEntity)
        {
            DbContext.ClientClaimPolicies.Remove(clientEntity);

            return await AutoSaveChangesAsync();
        }
    }
}
