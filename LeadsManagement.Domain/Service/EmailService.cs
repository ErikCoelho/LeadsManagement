namespace LeadsManagement.Domain.Service;

public class EmailService : IEmailService
{
    public void SendEmail(string email, string subject, string body)
    {
        Console.WriteLine($"Enviando email para {email} com assunto {subject} e corpo {body}");
    }
}