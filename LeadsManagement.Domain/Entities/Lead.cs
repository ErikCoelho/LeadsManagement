using LeadsManagement.Domain.Enums;
using LeadsManagement.Domain.Exceptions;

namespace LeadsManagement.Domain.Entities;

public class Lead : Entity
{
    public Lead(string firstName, string lastName, string email, string phoneNumber, 
        string description, string suburb, decimal price)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Description = description;
        Category = ECategoryType.Waiting;
        Suburb = suburb;
        Price = price;
        DataCriacao = DateTime.UtcNow;

        InvalidLeadExceptions.ThrowIfInvalid(firstName, price, email);
    }

    public string FirstName { get; private set; }  
    public string LastName { get; private set; }  
    public string Email { get; private set; }  
    public string PhoneNumber { get; private set; }  
    public string Description { get; private set; }  
    public ECategoryType Category { get; private set; }  
    public string Suburb { get; private set; }   
    public decimal Price { get; private set; }
    public DateTime DataCriacao { get; private set; }
}