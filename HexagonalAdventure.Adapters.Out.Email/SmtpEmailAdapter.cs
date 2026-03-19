using HexagonalAdventure.Application.Ports.Outbound;
using Microsoft.Extensions.Logging;

namespace HexagonalAdventure.Adapters.Out.Email;

/*
    Email gönderme işlemini üstlenen gerçek dış adaptörümüz.
    Tabii çalışma kapsamında şimdilik sadece loglama yapıyor ama gerçek bir SMTP istemcisi ile entegre edilebilir.
*/
public class SmtpEmailAdapter(ILogger<SmtpEmailAdapter> logger)
    : IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        logger.LogInformation("Sending email to {To} with subject {Subject}", to, subject);
        await Task.Delay(250); // Sembolik olarak email gönderme süresini simüle ediyoruz.
        logger.LogInformation("Email sent to {To} with subject {Subject}", to, subject);
    }
}
