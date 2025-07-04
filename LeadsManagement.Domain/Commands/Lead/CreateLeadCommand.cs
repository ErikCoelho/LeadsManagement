using LeadsManagement.Domain.Entities.Contracts;
using LeadsManagement.Domain.Exceptions;

namespace LeadsManagement.Domain.Commands.Lead;

public class CreateLeadCommand : ICommand
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Suburb { get; set; }  = string.Empty;
    public decimal Price { get; set; }
    public DateTime DataCriacao { get; set; }

    public void Validate()
    {
        InvalidLeadExceptions.ThrowIfInvalid(FirstName, Price, Email);
    }
}