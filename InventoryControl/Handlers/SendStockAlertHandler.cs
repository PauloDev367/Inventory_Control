using System.Net;
using System.Net.Mail;
using InventoryControl.Configurations;
using InventoryControl.Models;
using Microsoft.Extensions.Options;

namespace InventoryControl.Handlers;

public class SendStockAlertHandler
{
    private readonly SmtpSettings _smtpSettings;
    private readonly IWebHostEnvironment _env;

    public SendStockAlertHandler(IOptions<SmtpSettings> smtpSettings, IWebHostEnvironment env)
    {
        _smtpSettings = smtpSettings.Value;
        _env = env;
    }

    public async Task handleAsync(Product product, int quantity)
    {

        string htmlTemplatePath = Path.Combine(_env.ContentRootPath, "Templates", "ProductAlertTemplate.html");
        string htmlBody = await File.ReadAllTextAsync(htmlTemplatePath);

        htmlBody = htmlBody.Replace("{{Name}}", product.Name)
            .Replace("{{Id}}", product.Id.ToString());

        var mail = new MailMessage()
        {
            From = new MailAddress(_smtpSettings.Username),
            Subject = $"Low stock alert: Product {product.Name}",
            Body = htmlBody,
            IsBodyHtml = true
        };

        mail.To.Add(_smtpSettings.ToEmailAlert);

        using var smtp = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
        {
            Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
            EnableSsl = _smtpSettings.EnableSsl
        };

        await smtp.SendMailAsync(mail);
    }
}