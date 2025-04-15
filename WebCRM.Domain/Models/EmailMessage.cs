namespace WebCRM.Domain.Models;

public class EmailMessage
{
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public EmailMessageType Type { get; set; }
    public decimal? DiscountPercentage { get; set; }
} 