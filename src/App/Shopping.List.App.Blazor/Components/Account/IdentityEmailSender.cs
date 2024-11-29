using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Shopping.List.App.Blazor.Database.Auth;

namespace Shopping.List.App.Blazor.Components.Account;

public class AuthMessageSenderOptions
{
    public string SendGridKey { get; set; } = string.Empty;
}

public class IdentityEmailSender : IEmailSender<ApplicationUser>
{
    private readonly ILogger<IdentityEmailSender> _logger;
    private readonly AuthMessageSenderOptions _options;
    
    public IdentityEmailSender(ILogger<IdentityEmailSender> logger, IOptions<AuthMessageSenderOptions> optionsAccessor)
    {
        _logger = logger;
        _options = optionsAccessor.Value;
    }

    public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
        => SendEmailAsync(email, "Confirm your email", $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");

    public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
        => SendEmailAsync(email, "Reset your password", $"Please reset your password by <a href='{resetLink}'>clicking here</a>.");

    public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
        => SendEmailAsync(email, "Reset your password", $"Please reset your password using the following code: {resetCode}");

    private async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        if (string.IsNullOrWhiteSpace(_options.SendGridKey))
        {
            throw new Exception("Null SendGridKey");
        }
        
        await Execute(_options.SendGridKey, subject, htmlMessage, email);
    }
    
    private async Task Execute(string apiKey, string subject, string message, string toEmail)
    {
        var client = new SendGridClient(apiKey);
        var msg = new SendGridMessage
        {
            From = new EmailAddress("shopping-list@em9853.uknowmee.com", "Shopping List Team"),
            Subject = subject,
            PlainTextContent = message,
            HtmlContent = message
        };
        msg.AddTo(new EmailAddress(toEmail));

        msg.SetClickTracking(false, false);
        var response = await client.SendEmailAsync(msg);

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Email to {Email} queued successfully!", toEmail);
        }
        else
        {
            _logger.LogError("Failure Email to {Email}", toEmail);
        }
    }
}