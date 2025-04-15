namespace WebCRM.Domain.Options;

public class EmailOptions
{
    public string SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string FromEmail { get; set; }
    public bool EnableSsl { get; set; }
} 