using LeadsManagement.Domain.Entities;
using LeadsManagement.Domain.Enums;
using LeadsManagement.Domain.Repositories;
using LeadsManagement.Infra.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LeadsManagement.Infra.Respositories;

public class LeadRepository : ILeadRepository
{
    private readonly DataContext _context;

    public LeadRepository(DataContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Lead lead)
    {
        _context.Leads.Add(lead);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Lead>> GetAcceptedLeadsAsync(int pageNumber, int pageSize)
    {
        return await _context.Leads
            .Where(l => l.Category == ECategoryType.Accepted)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Lead> GetByIdAsync(Guid id)
    {
        var lead = await _context.Leads.FindAsync(id);
        if (lead == null)
        {
            throw new Exception("Lead not found");
        }
        return lead;
    }

    public async Task<IEnumerable<Lead>> GetWaitingLeadsAsync(int pageNumber, int pageSize)
    {
        return await _context.Leads
            .Where(l => l.Category == ECategoryType.Waiting)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task UpdateAsync(Lead lead)
    {
        _context.Leads.Update(lead);
        await _context.SaveChangesAsync();
    }
}
