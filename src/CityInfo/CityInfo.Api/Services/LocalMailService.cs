namespace CityInfo.Api.Services;

public class LocalMailService
{
    private string _mailTo = "admin@company.com";
    private string _mailFrom = "api@company.com";

    public void Send(string subject, string message)
    {
        Console.WriteLine($"Sending from {_mailFrom} to {_mailTo} using {nameof(LocalMailService)}");
        Console.WriteLine($"Subject: {subject}");
        Console.WriteLine($"Message: {message}");
    }
}
