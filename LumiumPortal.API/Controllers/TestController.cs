using Lumium.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LumiumPortal.API.Controllers;

public class TestController(ITenantContext tenantContext) : ControllerBase
{
    private readonly ITenantContext _tenantContext = tenantContext;

    [HttpGet("tenant-info")]
    public IActionResult GetTenantInfo()
    {
        return Ok(new
        {
            IsResolved = _tenantContext.IsResolved,
            TenantId = _tenantContext.TenantId,
            SchemaName = _tenantContext.SchemaName
        });
    }
}