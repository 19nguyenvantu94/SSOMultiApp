using System.Threading;
using System.Threading.Tasks;
using AuthenApi.Dtos.Dashboard;

namespace AuthenApi.Services.Interfaces;

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardIdentityServerAsync(int auditLogsLastNumberOfDays,
        CancellationToken cancellationToken = default);
}