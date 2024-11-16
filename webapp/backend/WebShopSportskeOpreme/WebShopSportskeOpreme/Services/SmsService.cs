using RestSharp;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using WebShopSportskeOpreme.Interfaces;


public class InfobipSmsService : ISmsService
{
    private readonly IConfiguration _configuration;
    private readonly RestClient _client;

    public InfobipSmsService(IConfiguration configuration)
    {
        _configuration = configuration;
        var options = new RestClientOptions("https://peyg6m.api.infobip.com")
        {
            MaxTimeout = -1,
        };
        _client = new RestClient(options);
    }

    public async Task SendSmsAsync(string phoneNumber, string message)
    {
        var request = new RestRequest("/sms/3/messages", Method.Post);

        request.AddHeader("Authorization", $"App {_configuration["Infobip:ApiKey"]}");
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Accept", "application/json");

        var messageContent = new
        {
            messages = new[]
            {
                new
                {
                    sender = "Web Shop",
                    destinations = new[]
                    {
                        new { to = phoneNumber }
                    },
                    content = new
                    {
                        text = message
                    }
                }
            }
        };

        var body = JsonSerializer.Serialize(messageContent);
        request.AddStringBody(body, DataFormat.Json);

        try
        {
            RestResponse response = await _client.ExecuteAsync(request);
            if (!response.IsSuccessful)
            {               
                throw new Exception($"Failed to send SMS. Status code: {response.StatusCode}, Content: {response.Content}");
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error sending SMS", ex);
        }
    }
}