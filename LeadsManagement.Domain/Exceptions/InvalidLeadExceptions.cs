using System.Text.RegularExpressions;

namespace LeadsManagement.Domain.Exceptions;

public class InvalidLeadExceptions: Exception
{
    private const string ValidateEmailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
    private const string DefaultErrorMessage = "Lead inválido";
    public InvalidLeadExceptions(string message = DefaultErrorMessage)
        :base(message)
    {
    }

    public static void ThrowIfInvalid(string name, decimal price, string email)
    {
        if(string.IsNullOrEmpty(name)) throw new InvalidLeadExceptions("O nome do lead não pode ser nulo");

        if (price < 0) throw new InvalidLeadExceptions("O preço do lead não pode ser menor que 0");
            
        if(!Regex.IsMatch(email, ValidateEmailPattern)) throw new InvalidLeadExceptions("O E-mail do lead não pode ser inválido");
    }
}