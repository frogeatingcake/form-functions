using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

public class SendEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<SendEmailService> _logger;

    public SendEmailService(IConfiguration configuration, ILogger<SendEmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;

    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var smtpServer = _configuration["SmtpServer"];
        var smtpPort = int.Parse(_configuration["SmtpPort"]);
        var emailUser = _configuration["EmailUser"];
        var emailPassword = _configuration["EmailPassword"];

        using (var client = new SmtpClient(smtpServer, smtpPort))
        {
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(emailUser, emailPassword);
            client.EnableSsl = true;

            var mailMessage = new MailMessage
            {
                From = new MailAddress(emailUser),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);

            _logger.LogInformation($"Sending email to {toEmail}");
            try
            {
                await client.SendMailAsync(mailMessage);
                _logger.LogInformation("Email sent successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending email: {ex.Message}");
                throw;
            }
        }

        _logger.LogInformation("Finished SendEmailAsync method");
    }
}

