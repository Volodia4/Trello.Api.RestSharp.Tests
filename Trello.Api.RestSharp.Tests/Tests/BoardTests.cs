using System.Net;
using RestSharp;
using Trello.Api.RestSharp.Tests.ApiClients;
using Trello.Api.RestSharp.Tests.Models;

namespace Trello.Api.RestSharp.Tests;

public class BoardTests : BaseTest
{
    private string _createdBoardId;
    private BoardClient _boardClient;

    [SetUp]
    public void InitClient()
    {
        _boardClient = new BoardClient(Client, Key, Token);
    }
    
    [Test]
    public async Task CreateBoard_Successfully()
    {
        string boardName =  "AutoTest_Board_" + DateTime.Now.Ticks;
        var response = await _boardClient.CreateBoard(boardName);
        
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        _createdBoardId = response.Data.Id;
        
        Assert.That(response.Data.Id, Is.Not.Null.And.Not.Empty, "Created board id can not be null or empty");
        Assert.That(response.Data.Name, Is.EqualTo(boardName), "Created board name does not match");
        
        TestContext.WriteLine($"Board created. Name: {response.Data.Name}, Id: {response.Data.Id}");
    }

    [Test]
    public async Task CreateBoard_WrongToken()
    {
        string boardName =  "AutoTest_Board_Wrong_" + DateTime.Now.Ticks;
        var response = await _boardClient.CreateBoard(boardName, "wrong_token");
        
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        Assert.That(response.Data, Is.Null);
        
        TestContext.WriteLine("Success. Board with wrong token was not created");
    }

    [Test]
    public async Task CreateBoard_EmptyName()
    {
        var response = await _boardClient.CreateBoard(null);
        
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        Assert.That(response.Data, Is.Null);
        
        TestContext.WriteLine("Success. Board with empty name was not created");
    }

    [Test]
    public async Task GetBoard_Successfully()
    {
        string boardName =  "AutoTest_Board_" + DateTime.Now.Ticks;
        var response = await _boardClient.CreateBoard(boardName);
        
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        _createdBoardId = response.Data.Id;
        
        var retrievedBoard = await _boardClient.GetBoard(_createdBoardId);
        
        Assert.That(retrievedBoard.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(retrievedBoard.Data.Id, Is.EqualTo(_createdBoardId), "Retrieved board id does not match");
        Assert.That(retrievedBoard.Data.Name, Is.EqualTo(boardName), "Retrieved board name does not match");
        
        TestContext.WriteLine($"Board {retrievedBoard.Data.Name} found");
    }

    [Test]
    public async Task GetBoard_WrongId()
    {
        var response = await _boardClient.GetBoard("123456789012345678901234");
        
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        
        TestContext.WriteLine("Success. Board with wrong id was not found");
    }

    [Test]
    public async Task UpdateBoard_Successfully()
    {
        string boardName = "AutoTest_Board_" + DateTime.Now.Ticks;
        var response = await _boardClient.CreateBoard(boardName);
        
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        _createdBoardId = response.Data.Id;
        
        string newBoardName = "AutoTest_Board_Changed_" + DateTime.Now.Ticks;
        var changedBoard = await _boardClient.UpdateBoard(_createdBoardId, newBoardName);
        
        Assert.That(changedBoard.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(changedBoard.Data.Id, Is.EqualTo(_createdBoardId), "Changed board id does not match");
        Assert.That(changedBoard.Data.Name, Is.EqualTo(newBoardName), "Changed board name does not match");
        
        TestContext.WriteLine($"Board updated. New name: {changedBoard.Data.Name}");
    }
    
    [TearDown]
    public async Task CleanUp()
    {
        if (!string.IsNullOrEmpty(_createdBoardId))
        {
            var response = await _boardClient.DeleteBoard(_createdBoardId);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                TestContext.WriteLine($"Board {_createdBoardId} deleted");
            }
        }
    }
}
