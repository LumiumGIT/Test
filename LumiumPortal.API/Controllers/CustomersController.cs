using Lumium.Application.Features.Customers.Commands.CreateCustomer;
using Lumium.Application.Features.Customers.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LumiumPortal.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class CustomersController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customers = await sender.Send(new GetCustomersQuery());
        return Ok(customers);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCustomerCommand command)
    {
        var customer = await sender.Send(command);
        return CreatedAtAction(nameof(GetAll), new { id = customer.Id }, customer);
    }
}