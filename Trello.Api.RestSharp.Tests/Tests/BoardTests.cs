using System.Net;
using RestSharp;

namespace Trello.Api.RestSharp.Tests;

public class BoardTests : BaseTest
{
    [Test]
    public async Task CreateBoard_Successfully()
    {
        var request = new RestRequest("/1/boards/", Method.Post);
        request.AddQueryParameter("key", Key);
        request.AddQueryParameter("token", Token);
        
        string boardName = "AutoTest_Board_" + DateTime.Now.Ticks;
        request.AddQueryParameter("name", boardName);
        
        var response = await Client.ExecuteAsync(request);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK),
            $"Expected status code 200, but got status code {response.StatusCode}. Response {response.Content}");
        TestContext.WriteLine("Created board: " + boardName);
    }
}
