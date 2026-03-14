using Domain.Enums.Contracts;
using Domain.Enums.Shared;
using Lumium.Application.Common.Extensions;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Features.Dashboard.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Features.Dashboard.Queries;

public record GetDashboardDataQuery : IRequest<DashboardDataDto>;

public class GetDashboardDataQueryHandler(IApplicationDbContextFactory contextFactory)
    : IRequestHandler<GetDashboardDataQuery, DashboardDataDto>
{
    public async Task<DashboardDataDto> Handle(GetDashboardDataQuery request, CancellationToken cancellationToken)
    {
        return await contextFactory.ExecuteInContextAsync(async context =>
        {
            var alerts = await GetAlertsAsync(context, cancellationToken);
            var stats = await GetStatsAsync(context, cancellationToken);
            var upcomingDeadlines = await GetUpcomingDeadlinesAsync(context, cancellationToken);

            return new DashboardDataDto
            {
                Alerts = alerts,
                Stats = stats,
                UpcomingDeadlines = upcomingDeadlines
            };
        }, cancellationToken);
    }

    private static async Task<DashboardAlertsDto> GetAlertsAsync(IApplicationDbContext context,
        CancellationToken cancellationToken)
    {
        var today = DateTime.Today;

        var expiredCertificates = await context.Certificates
            .Where(c => c.ExpiryDate < today)
            .Include(c => c.Client)
            .OrderBy(c => c.ExpiryDate)
            .Select(c => new ExpiredCertificateAlertDto
            {
                CertificateId = c.Id,
                ClientId = c.ClientId,
                ClientName = c.Client.Name,
                CertificateName = c.CertificateName,
                ExpiryDate = c.ExpiryDate,
                DaysExpired = (today - c.ExpiryDate).Days
            })
            .ToListAsync(cancellationToken);

        var expiredContracts = await context.Contracts
            .Where(c => c.EndDate.HasValue && c.EndDate.Value < today && c.Status == ContractStatus.Active)
            .Include(c => c.Client)
            .OrderBy(c => c.EndDate)
            .Select(c => new ExpiredContractAlertDto
            {
                ContractId = c.Id,
                ClientId = c.ClientId,
                ClientName = c.Client.Name,
                ContractNumber = c.ContractNumber,
                EndDate = c.EndDate,
                DaysExpired = (today - c.EndDate!.Value).Days
            })
            .ToListAsync(cancellationToken);

        var clientsWithoutDocs = await context.Clients
            .Where(c => !c.Documents.Any())
            .Select(c => new ClientWithoutDocumentsAlertDto
            {
                ClientId = c.Id,
                ClientName = c.Name,
                CertificatesCount = c.Certificates.Count,
                ContractsCount = c.Contracts.Count
            })
            .OrderBy(c => c.ClientName)
            .ToListAsync(cancellationToken);

        return new DashboardAlertsDto
        {
            ExpiredCertificates = expiredCertificates,
            ExpiredContracts = expiredContracts,
            ClientsWithoutDocuments = clientsWithoutDocs
        };
    }

    private static async Task<DashboardStatsDto> GetStatsAsync(
        IApplicationDbContext context,
        CancellationToken cancellationToken)
    {
        var today = DateTime.Today;
        var startOfMonth = new DateTime(today.Year, today.Month, 1);
        var startOfWeek = today.AddDays(-(int)today.DayOfWeek);

        return new DashboardStatsDto
        {
            TotalClients = await context.Clients.CountAsync(cancellationToken),

            NewClientsThisMonth = await context.Clients
                .CountAsync(c => c.CreatedAt >= startOfMonth, cancellationToken),

            TotalCertificates = await context.Certificates.CountAsync(cancellationToken),

            CertificatesExpiringSoon = await context.Certificates
                .CountAsync(c => c.ExpiryDate >= today && c.ExpiryDate <= today.AddDays(30), cancellationToken),

            TotalContracts = await context.Contracts.CountAsync(cancellationToken),

            ActiveContracts = await context.Contracts
                .CountAsync(c => c.Status == ContractStatus.Active, cancellationToken),

            TotalDocuments = await context.Documents.CountAsync(cancellationToken),

            DocumentsThisWeek = await context.Documents
                .CountAsync(d => d.UploadedAt >= startOfWeek, cancellationToken)
        };
    }

    private static async Task<List<UpcomingDeadlineDto>> GetUpcomingDeadlinesAsync(
        IApplicationDbContext context,
        CancellationToken cancellationToken)
    {
        var today = DateTime.Today;
        var next30Days = today.AddDays(30);

        var certificateDeadlines = await GetCertificateDeadlinesAsync(context, today, next30Days, cancellationToken);
        var contractDeadlines = await GetContractDeadlinesAsync(context, today, next30Days, cancellationToken);

        return certificateDeadlines
            .Concat(contractDeadlines)
            .OrderBy(d => d.Date)
            .Take(10)
            .ToList();
    }

    private static async Task<List<UpcomingDeadlineDto>> GetCertificateDeadlinesAsync(
        IApplicationDbContext context,
        DateTime today,
        DateTime endDate,
        CancellationToken cancellationToken)
    {
        return await context.Certificates
            .Where(c => c.ExpiryDate >= today && c.ExpiryDate <= endDate)
            .Include(c => c.Client)
            .OrderBy(c => c.ExpiryDate)
            .Take(10)
            .Select(c => new UpcomingDeadlineDto
            {
                Date = c.ExpiryDate,
                Type = DeadlineType.Certificate,
                ItemId = c.Id,
                ClientId = c.ClientId,
                Title = c.CertificateName,
                ClientName = c.Client.Name,
                DaysRemaining = (c.ExpiryDate - today).Days,
                Status = (c.ExpiryDate - today).Days <= 7 ? DeadlineStatus.Critical :
                    (c.ExpiryDate - today).Days <= 14 ? DeadlineStatus.Warning :
                    DeadlineStatus.Info
            })
            .ToListAsync(cancellationToken);
    }

    private static async Task<List<UpcomingDeadlineDto>> GetContractDeadlinesAsync(
        IApplicationDbContext context,
        DateTime today,
        DateTime endDate,
        CancellationToken cancellationToken)
    {
        return await context.Contracts
            .Where(c => c.EndDate.HasValue &&
                        c.EndDate.Value >= today &&
                        c.EndDate.Value <= endDate &&
                        c.Status == ContractStatus.Active)
            .Include(c => c.Client)
            .OrderBy(c => c.EndDate)
            .Take(10)
            .Select(c => new UpcomingDeadlineDto
            {
                Date = c.EndDate!.Value,
                Type = DeadlineType.Contract,
                ItemId = c.Id,
                ClientId = c.ClientId,
                Title = c.ContractNumber,
                ClientName = c.Client.Name,
                DaysRemaining = (c.EndDate!.Value - today).Days,
                Status = (c.EndDate!.Value - today).Days <= 7 ? DeadlineStatus.Critical :
                    (c.EndDate!.Value - today).Days <= 14 ? DeadlineStatus.Warning :
                    DeadlineStatus.Info
            })
            .ToListAsync(cancellationToken);
    }
}