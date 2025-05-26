using System;

namespace AuthenApi.Dtos.Dashboard;

public class DashboardAuditLogDto
{
    public int Total { get; set; }

    public DateTime Created { get; set; }
}