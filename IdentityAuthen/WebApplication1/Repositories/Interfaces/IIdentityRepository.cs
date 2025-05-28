// Copyright (c) Jan Škoruba. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Authen.Users.Models;
using AuthenApi.Common;
using AuthenApi.Dtos.Identity;
using Microsoft.AspNetCore.Identity;

namespace Authen.Repositories.Interfaces
{
    public interface IIdentityRepository
    {
        Task<bool> ExistsUserAsync(string userId);

        Task<bool> ExistsRoleAsync(string roleId);

        Task<PagedList<ApplicationUser>> GetUsersAsync(string search, int page = 1, int pageSize = 10);

        Task<PagedList<ApplicationUser>> GetRoleUsersAsync(string roleId, string search, int page = 1, int pageSize = 10);

        Task<PagedList<ApplicationUser>> GetClaimUsersAsync(string claimType, string claimValue, int page = 1, int pageSize = 10);

        Task<PagedList<ApplicationRole>> GetRolesAsync(string search, int page = 1, int pageSize = 10);

        Task<(IdentityResult identityResult, Guid roleId)> CreateRoleAsync(ApplicationRole role);

        Task<ApplicationRole> GetRoleAsync(Guid roleId);

        Task<List<ApplicationRole>> GetRolesAsync();

        Task<(IdentityResult identityResult, Guid roleId)> UpdateRoleAsync(ApplicationRole role);

        Task<ApplicationUser> GetUserAsync(string userId);

        Task<(IdentityResult identityResult, Guid userId)> CreateUserAsync(ApplicationUser user);

        Task<(IdentityResult identityResult, Guid userId)> UpdateUserAsync(ApplicationUser user);

        Task<IdentityResult> DeleteUserAsync(string userId);

        Task<IdentityResult> CreateUserRoleAsync(string userId, string roleId);

        Task<PagedList<ApplicationRole>> GetUserRolesAsync(string userId, int page = 1, int pageSize = 10);

        Task<IdentityResult> DeleteUserRoleAsync(string userId, string roleId);

        Task<PagedList<ApplicationUserClaim>> GetUserClaimsAsync(string userId, int page = 1, int pageSize = 10);

        Task<ApplicationUserClaim> GetUserClaimAsync(string userId, Guid claimId);

        Task<IdentityResult> CreateUserClaimsAsync(ApplicationUserClaim claims);

        Task<IdentityResult> UpdateUserClaimsAsync(ApplicationUserClaim claims);

        Task<IdentityResult> DeleteUserClaimAsync(string userId, Guid claimId);

        Task<List<UserLoginInfo>> GetUserProvidersAsync(string userId);

        Task<IdentityResult> DeleteUserProvidersAsync(string userId, string providerKey, string loginProvider);

        Task<IdentityUserLogin<Guid>> GetUserProviderAsync(string userId, string providerKey);

        Task<IdentityResult> UserChangePasswordAsync(string userId, string password);

        Task<IdentityResult> CreateRoleClaimsAsync(ApplicationRoleClaim claims);

        Task<IdentityResult> UpdateRoleClaimsAsync(ApplicationRoleClaim claims);

        Task<PagedList<ApplicationRoleClaim>> GetRoleClaimsAsync(string roleId, int page = 1, int pageSize = 10);

        Task<PagedList<ApplicationRoleClaim>> GetUserRoleClaimsAsync(string userId, string claimSearchText, int page = 1, int pageSize = 10);

        Task<ApplicationRoleClaim> GetRoleClaimAsync(string roleId, Guid claimId);

        Task<IdentityResult> DeleteRoleClaimAsync(string roleId, Guid claimId);

        Task<IdentityResult> DeleteRoleAsync(ApplicationRole role);

        bool AutoSaveChanges { get; set; }

        Task<int> SaveAllChangesAsync();
    }
}