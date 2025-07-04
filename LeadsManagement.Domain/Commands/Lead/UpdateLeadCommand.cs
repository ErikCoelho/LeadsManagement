using LeadsManagement.Domain.Entities.Contracts;
using LeadsManagement.Domain.Enums;

namespace LeadsManagement.Domain.Commands.Lead;

public class UpdateLeadCommand : ICommand
{
    public Guid? Id { get; set; }
    public ECategoryType Category { get; set; }

    public void Validate()
    {
        if (Id == Guid.Empty)
        {
            throw new Exception("Id do lead não pode ser vazio");
        }
    }
}
