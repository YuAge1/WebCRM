using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using WebCRM.Domain.Models;
using WebCRM.Domain.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace WebCRM.Application.Services;

public interface IEmailService
{
    Task SendEmail(EmailMessage message);
    Task SendDiscountNotification(string email, decimal discountPercentage);
    Task SendLoginNotification(string email);
    Task SendPromotionalEmail(string email, string subject, string body);
}

public class EmailService : IEmailService
{
    private readonly EmailOptions _emailOptions;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailOptions> emailOptions, ILogger<EmailService> logger)
    {
        _emailOptions = emailOptions.Value;
        _logger = logger;
    }

    public async Task SendEmail(EmailMessage message)
    {
        try
        {
            _logger.LogInformation("Starting email sending process to {To}", message.To);
            _logger.LogInformation("SMTP Server: {Server}, Port: {Port}, SSL: {SSL}, Username: {Username}", 
                _emailOptions.SmtpServer, _emailOptions.SmtpPort, _emailOptions.EnableSsl, _emailOptions.Username);

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("WebCRM", _emailOptions.FromEmail));
            email.To.Add(new MailboxAddress("", message.To));
            email.Subject = message.Subject;

            var builder = new BodyBuilder { HtmlBody = message.Body };
            email.Body = builder.ToMessageBody();

            _logger.LogInformation("Mail message prepared. From: {From}, To: {To}, Subject: {Subject}", 
                _emailOptions.FromEmail, message.To, message.Subject);

            using var client = new SmtpClient();
            
            // Устанавливаем таймаут подключения
            client.Timeout = 60000; // Увеличиваем таймаут до 60 секунд
            
            // Отключаем проверку сертификата
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;
            
            // Подключаемся к серверу
            await client.ConnectAsync(_emailOptions.SmtpServer, _emailOptions.SmtpPort, SecureSocketOptions.Auto);
            _logger.LogInformation("Connected to SMTP server");
            
            try
            {
                // Аутентифицируемся
                await client.AuthenticateAsync(_emailOptions.Username, _emailOptions.Password);
                _logger.LogInformation("Authenticated successfully");
                
                // Отправляем письмо
                await client.SendAsync(email);
                _logger.LogInformation("Email sent successfully to {To}", message.To);
            }
            catch (AuthenticationException ex)
            {
                _logger.LogError("Authentication failed: {Message}", ex.Message);
                throw;
            }
            catch (SmtpCommandException ex)
            {
                _logger.LogError("SMTP command failed: {Message}, StatusCode: {StatusCode}", 
                    ex.Message, ex.StatusCode);
                throw;
            }
            finally
            {
                // Отключаемся от сервера
                if (client.IsConnected)
                {
                    await client.DisconnectAsync(true);
                    _logger.LogInformation("Disconnected from SMTP server");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email to {To}: {Message}", message.To, ex.Message);
            throw;
        }
    }

    public async Task SendDiscountNotification(string email, decimal discountPercentage)
    {
        try
        {
            _logger.LogInformation("Sending discount notification to {Email}", email);
            var message = new EmailMessage
            {
                To = email,
                Subject = "Специальное предложение для вас!",
                Body = $@"
                    <h2>Уважаемый клиент!</h2>
                    <p>Мы рады сообщить, что вы получили специальную скидку {discountPercentage}% на все заказы!</p>
                    <p>Спешите воспользоваться предложением!</p>
                    <p>С уважением,<br>Команда WebCRM</p>",
                Type = EmailMessageType.Discount,
                DiscountPercentage = discountPercentage
            };

            await SendEmail(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending discount notification to {Email}", email);
            throw;
        }
    }

    public async Task SendLoginNotification(string email)
    {
        try
        {
            _logger.LogInformation("Sending login notification to {Email}", email);
            var message = new EmailMessage
            {
                To = email,
                Subject = "Успешный вход в систему",
                Body = $@"
                    <h2>Уважаемый пользователь!</h2>
                    <p>Вы успешно вошли в систему WebCRM.</p>
                    <p>Если это были не вы, пожалуйста, свяжитесь с нами.</p>
                    <p>С уважением,<br>Команда WebCRM</p>",
                Type = EmailMessageType.Login
            };

            await SendEmail(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending login notification to {Email}", email);
            throw;
        }
    }

    public async Task SendPromotionalEmail(string email, string subject, string body)
    {
        try
        {
            _logger.LogInformation("Sending promotional email to {Email}", email);
            var message = new EmailMessage
            {
                To = email,
                Subject = subject,
                Body = body,
                Type = EmailMessageType.Promotional
            };

            await SendEmail(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending promotional email to {Email}", email);
            throw;
        }
    }
} 