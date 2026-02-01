using Domain.Entities.Portal;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Features.Customers.DTOs;
using MediatR;

namespace Lumium.Application.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerCommand : IRequest<CustomerDto>
{
    public string Name { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string? Phone { get; init; }
    public string? Address { get; init; }
}

public class CreateCustomerCommandHandler(
    IApplicationDbContext context,
    ITenantContext tenantContext)
    : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    public async Task<CustomerDto> Handle(
        CreateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            TenantId = tenantContext.TenantId!, // ← Automatski iz tenant context
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        context.Customers.Add(customer);
        await context.SaveChangesAsync(cancellationToken);

        return new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            Address = customer.Address,
            IsActive = customer.IsActive,
            CreatedAt = customer.CreatedAt
        };
    }
}