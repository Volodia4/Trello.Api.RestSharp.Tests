using Microsoft.Extensions.Configuration;
using RestSharp;

namespace Trello.Api.RestSharp.Tests;

public class BaseTest
{
    protected string Key;
    protected string Token;
    protected RestClient Client;

    [SetUp]
    public void Setup()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        
        Key = config["Trello:Key"];
        Token = config["Trello:Token"];
        var baseUrl = config["Trello:BaseUrl"];
        Client = new RestClient(baseUrl);
    }

    [TearDown]
    public void TearDown()
    {
        Client.Dispose();
    }
}
