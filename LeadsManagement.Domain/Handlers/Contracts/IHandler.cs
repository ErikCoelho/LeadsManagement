using LeadsManagement.Domain.Entities.Contracts;

namespace LeadsManagement.Domain.Handlers.Contracts;

public interface IHandler<T> where T : ICommand
{
    Task<ICommandResult> HandleAsync(T command);
}