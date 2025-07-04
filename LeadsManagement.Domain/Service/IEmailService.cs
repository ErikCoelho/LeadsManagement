namespace LeadsManagement.Domain.Service;

public interface IEmailService
{
    void SendEmail(string email, string subject, string body);
} 