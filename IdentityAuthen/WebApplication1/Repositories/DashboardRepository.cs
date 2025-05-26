using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AuthenApi.Entities;
using AuthenApi.Repositories.Interfaces;
using Authen.Data;

namespace AuthenApi.Repositories;

public class DashboardRepository : IDashboardRepository
{
    protected readonly ApplicationDbContext DbContext;

    public DashboardRepository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<DashboardDataView> GetDashboardIdentityServerAsync(int auditLogsLastNumberOfDays, CancellationToken cancellationToken = default)
    {
        return new DashboardDataView
        {
            ClientsTotal = await DbContext.Clients.CountAsync(cancellationToken: cancellationToken),
            ApiResourcesTotal = await DbContext.ApiResources.CountAsync(cancellationToken: cancellationToken),
            ApiScopesTotal = await DbContext.ApiScopes.CountAsync(cancellationToken: cancellationToken),
            IdentityResourcesTotal = await DbContext.IdentityResources.CountAsync(cancellationToken: cancellationToken),
            IdentityProvidersTotal = await DbContext.IdentityProviders.CountAsync(cancellationToken: cancellationToken)
        };
    }
}