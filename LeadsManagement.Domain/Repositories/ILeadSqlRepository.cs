using LeadsManagement.Domain.Entities;

namespace LeadsManagement.Domain.Repositories;

public interface ILeadSqlRepository
{
    Task<IEnumerable<Lead>> GetWaitingLeadsAsync(int pageNumber, int pageSize);
    Task<IEnumerable<Lead>> GetAcceptedLeadsAsync(int pageNumber, int pageSize);
    Task<Lead> GetByIdAsync(Guid id);
    Task CreateAsync(Lead lead);
    Task UpdateAsync(Lead lead);
}