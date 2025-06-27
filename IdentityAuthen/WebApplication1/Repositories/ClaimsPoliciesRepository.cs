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
using Polly;
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

            // Remove .Include(x => x.PolicyRoles) and use a manual join to get roles if needed

            var clients = await (
                from policy in DbContext.ClientClaimPolicies
                join client in DbContext.Clients on policy.IdClient equals client.Id
                where string.IsNullOrEmpty(search) || client.ClientId.Contains(search)
                orderby policy.Id
                select new ClientPageDto
                {
                    Id = policy.Id,
                    ClientId = client.Id,
                    ClientName = client.ClientId,
                    RolesNames = string.Join(", ",
                        DbContext.ClientClaimPolicyRoles
                            .Where(r => r.ClientClaimPolicyId == policy.Id)
                            .Select(r => r.Role.Name))
                }
            )
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            foreach (var client in clients)
            {
                var clientData = await DbContext.Clients
                    .Where(c => c.Id == client.ClientId)
                    .Select(c => new { c.ClientId })
                    .FirstOrDefaultAsync();
                if (clientData != null)
                {
                    client.ClientName = clientData.ClientId;
                }
            }

            pagedList.ClientsPolicies.AddRange(clients);
            pagedList.TotalCount = await DbContext.ClientClaimPolicies
                .WhereIf(!string.IsNullOrEmpty(search), x => x.Client.ClientId.Contains(search))
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

            await DbContext.SaveChangesAsync(); // Save to generate Id

            return dataAdd.Entity.Id;
        }

        public async Task<ClientsIdDto> ClaimsPoliciesById(int identityResourcePropertiesDto)
        {

            var data = await (
            from policy in DbContext.ClientClaimPolicies
            join client in DbContext.Clients on policy.IdClient equals client.Id
            where policy.Id == identityResourcePropertiesDto
            orderby policy.Id
            select new ClientsIdDto
            {
                Id = policy.Id,
                Client = client,
            }
            ).FirstOrDefaultAsync();

            if (data == null)
            {
                data = new ClientsIdDto();
            }


            var getAllClients = await DbContext.ClientClaimPolicies.Select(x => x.IdClient).ToListAsync();

            // Fix for CS7036: Ensure the SelectItem constructor is called with the required parameters 'id' and 'text'.

            data.ClientsList = DbContext.Clients
                .Where(x => !getAllClients.Contains(x.Id))
                .Select(c => new SelectItem(c.Id.ToString(), c.ClientId))
                .ToList();

            if (data.Id != 0)
            {
                var dataPolicyRoles = DbContext.ClientClaimPolicyRoles.Where(x => x.ClientClaimPolicyId == data.Id).Include(x => x.Role).Select(x => new SelectItem(x.Id.ToString(), x.Role!.Name)).ToList();

                data.Roles = dataPolicyRoles.ToList();

                var dataPolicyRolesRemove = DbContext.ClientClaimPolicyRoles.Where(x => x.ClientClaimPolicyId == data.Id).Include(x => x.Role).Select(x => new SelectItem(x.Role.Id.ToString(), x.Role!.Name)).ToList();

                var roleIds = dataPolicyRolesRemove.Select(r => r.Id).ToList();

                var roles = await DbContext.Roles
         .Where(x => !roleIds.Contains(x.Id.ToString()))
         .Select(r => new SelectItem(r.Id.ToString(), r.Name))
         .ToListAsync();

                data.RolesList = roles;
            }
            else
            {
                data.RolesList = DbContext.Roles
                                .ToList()
                                .Select(r => new SelectItem(r.Id.ToString(), r.Name)).ToList();
            }
            return data;
        }

        public virtual async Task<int> UpdateIdentityResourceAsync(ClientsIdDto identityResource)
        {

            //Update with new data
            //DbContext.ClientClaimPolicies.Update(identityResource);

            await DbContext.ClientClaimPolicyRoles.AddAsync(new ClientClaimPolicyRole
            {
                ClientClaimPolicyId = identityResource.Id,
                RoleId = identityResource.RoleId,

            });

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

        public virtual async Task<int> DeleteEntityRole(int roleId)
        {

            var clientEntity = await DbContext.ClientClaimPolicyRoles
                .Where(x => x.Id == roleId)
                .FirstOrDefaultAsync();

            DbContext.ClientClaimPolicyRoles.Remove(clientEntity!);

            return await AutoSaveChangesAsync();
        }

        public async Task<ClientsRolesDeleteDto> GetClientsRolesDeleteDto(int identityResourceId)
        {
            var clientEntity = await DbContext.ClientClaimPolicyRoles
               .Where(x => x.Id == identityResourceId)
               .Include(x=>x.Role)
               .Include(x=>x.ClientClaimPolicy)
               .FirstOrDefaultAsync();

            return new ClientsRolesDeleteDto
            {
                Id = clientEntity!.Id,
                ClaimPolicyId = clientEntity.ClientClaimPolicyId,
                RoleName = clientEntity!.Role!.Name,
                ClientName = DbContext.Clients
                    .Where(c => c.Id == clientEntity.ClientClaimPolicy!.IdClient)
                    .Select(c => c.ClientId)
                    .FirstOrDefault() ?? string.Empty
            };
        }
    }
}
