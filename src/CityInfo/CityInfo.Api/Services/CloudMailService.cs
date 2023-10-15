using Microsoft.Extensions.Configuration;

namespace CityInfo.Api.Services;

public class CloudMailService : IMailService
{
    private readonly string _mailTo = string.Empty;
    private readonly string _mailFrom = string.Empty;

    public CloudMailService(IConfiguration configuration)
    {
        _ = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _mailTo = configuration["mailSettings:mailToAddress"];
        _mailFrom = configuration["mailSettings:mailFromAddress"];
    }

    public void Send(string subject, string message)
    {
        Console.WriteLine($"Sending from {_mailFrom} to {_mailTo} using {nameof(CloudMailService)}");
        Console.WriteLine($"Subject: {subject}");
        Console.WriteLine($"Message: {message}");
    }
}
