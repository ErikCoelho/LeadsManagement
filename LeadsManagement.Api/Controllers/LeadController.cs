using LeadsManagement.Domain.Commands;
using LeadsManagement.Domain.Commands.Lead;
using LeadsManagement.Domain.Entities;
using LeadsManagement.Domain.Handlers;
using LeadsManagement.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LeadsManagement.Api.Controllers;

[ApiController]
public class LeadController: ControllerBase
{
    [HttpGet("v1/leads/accepted")]
    public async Task<IEnumerable<Lead>> GetAcceptedLeads(
        [FromQuery] int pageNumber, 
        [FromQuery] int pageSize, 
        [FromServices] ILeadRepository leadRepository)
    {
        var leads = await leadRepository.GetAcceptedLeadsAsync(pageNumber, pageSize);
        return leads;
    }

    [HttpGet("v1/leads/waiting")]
    public async Task<IEnumerable<Lead>> GetWaitingLeads(
        [FromQuery] int pageNumber, 
        [FromQuery] int pageSize, 
        [FromServices] ILeadRepository leadRepository)
    {
        var leads = await leadRepository.GetWaitingLeadsAsync(pageNumber, pageSize);
        return leads;
    }

    [HttpPut("v1/leads/{id}")]
    public async Task<GenericCommandResult> UpdateLead([FromRoute] Guid id, 
    [FromBody] UpdateLeadCommand request, 
    [FromServices] LeadHandler handler)
    {
        request.Id = id;
        var result = await handler.HandleAsync(request);
        return (GenericCommandResult)result;
    }
}