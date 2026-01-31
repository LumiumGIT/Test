using Lumium.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LumiumPortal.API.Controllers;

[ApiController]
[Route("api/test")]
public class TestController(ITenantContext tenantContext) : ControllerBase
{
    [HttpGet("tenant-info")]
    public IActionResult GetTenantInfo()
    {
        return Ok(new
        {
            IsResolved = tenantContext.IsResolved,
            TenantId = tenantContext.TenantId,
            SchemaName = tenantContext.SchemaName
        });
    }
}