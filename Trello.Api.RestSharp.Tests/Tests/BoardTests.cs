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
        var createdBoard = await _boardClient.CreateBoard(boardName);
        
        _createdBoardId = createdBoard.Id;
        
        Assert.That(createdBoard.Name, Is.EqualTo(boardName), "Created board name does not match");
        Assert.That(createdBoard.Id, Is.Not.Null.And.Not.Empty, "Created board id can not be null or empty");
        
        TestContext.WriteLine($"Board created. Name: {createdBoard.Name}, Id: {createdBoard.Id}");
    }

    [Test]
    public async Task GetBoard_Successfully()
    {
        string boardName =  "AutoTest_Board_" + DateTime.Now.Ticks;
        var createdBoard = await _boardClient.CreateBoard(boardName);
        
        _createdBoardId = createdBoard.Id;
        
        var retrievedBoard = await _boardClient.GetBoard(_createdBoardId);
        
        Assert.That(retrievedBoard.Name, Is.EqualTo(boardName), "Retrieved board name does not match");
        Assert.That(retrievedBoard.Id, Is.EqualTo(_createdBoardId), "Retrieved board id does not match");
        
        TestContext.WriteLine($"Board {_createdBoardId} retrieved");
    }
    
    [TearDown]
    public async Task CleanUp()
    {
        if (!string.IsNullOrEmpty(_createdBoardId))
        {
            var statusCode = await _boardClient.DeleteBoard(_createdBoardId);
            if (statusCode == HttpStatusCode.OK)
            {
                TestContext.WriteLine($"Board {_createdBoardId} deleted");
            }
        }
    }
}
