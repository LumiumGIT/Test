using System.Data;
using System.Data.Common;
using Lumium.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Lumium.Infrastructure.Interceptors;

public class TenantConnectionInterceptor(ITenantContext tenantContext) : DbConnectionInterceptor
{
    public override async Task ConnectionOpenedAsync(
        DbConnection connection,
        ConnectionEndEventData eventData,
        CancellationToken cancellationToken = default)
    {
        await SetSearchPathAsync(connection, cancellationToken);
        await base.ConnectionOpenedAsync(connection, eventData, cancellationToken);
    }

    public override void ConnectionOpened(
        DbConnection connection,
        ConnectionEndEventData eventData)
    {
        SetSearchPath(connection);
        base.ConnectionOpened(connection, eventData);
    }

    private async Task SetSearchPathAsync(DbConnection connection, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(tenantContext.SchemaName))
            return;

        if (connection.State != ConnectionState.Open)
            return;

        await using var command = connection.CreateCommand();
        command.CommandText = $"SET search_path TO {tenantContext.SchemaName}";
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private void SetSearchPath(DbConnection connection)
    {
        if (string.IsNullOrEmpty(tenantContext.SchemaName))
            return;

        if (connection.State != ConnectionState.Open)
            return;

        using var command = connection.CreateCommand();
        command.CommandText = $"SET search_path TO {tenantContext.SchemaName}";
        command.ExecuteNonQuery();
    }
}