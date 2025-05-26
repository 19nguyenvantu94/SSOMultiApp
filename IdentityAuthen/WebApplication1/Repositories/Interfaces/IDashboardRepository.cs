using System.Threading;
using System.Threading.Tasks;
using AuthenApi.Entities;

namespace AuthenApi.Repositories.Interfaces;

public interface IDashboardRepository
{
    Task<DashboardDataView> GetDashboardIdentityServerAsync(int auditLogsLastNumberOfDays,
        CancellationToken cancellationToken = default);
}