namespace LeadsManagement.Domain.Entities.Contracts;

public interface ICommand
{
    void Validate();
}