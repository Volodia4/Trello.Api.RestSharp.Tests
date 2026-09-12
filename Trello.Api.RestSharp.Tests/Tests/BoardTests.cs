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
    public async Task CreateBoard_WrongToken()
    {
        string boardName =  "AutoTest_Board_Wrong_" + DateTime.Now.Ticks;
        var wrongBoardStatus = await _boardClient.CreateBoard_WrongToken(boardName);
        
        Assert.That(wrongBoardStatus, Is.EqualTo(HttpStatusCode.Unauthorized));
        
        TestContext.WriteLine("Success. Board with wrong token was not created");
    }

    [Test]
    public async Task CreateBoard_EmptyName()
    {
        var wrongBoardStatus = await _boardClient.CreateBoard_EmptyName();
        
        Assert.That(wrongBoardStatus, Is.EqualTo(HttpStatusCode.BadRequest));
        
        TestContext.WriteLine("Success. Board with empty name was not created");
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
        
        TestContext.WriteLine($"Board {retrievedBoard.Name} found");
    }

    [Test]
    public async Task GetBoard_WrongId()
    {
        var wrongRetrievedBoardStatus = await _boardClient.GetBoardWrongId();
        
        Assert.That(wrongRetrievedBoardStatus,  Is.EqualTo(HttpStatusCode.NotFound));
        
        TestContext.WriteLine("Success. Board with wrong id was not found");
    }

    [Test]
    public async Task UpdateBoard_Successfully()
    {
        string boardName =  "AutoTest_Board_" + DateTime.Now.Ticks;
        var createdBoard = await _boardClient.CreateBoard(boardName);
        
        _createdBoardId = createdBoard.Id;
        
        string newBoardName =  "AutoTest_Board_Changed_" + DateTime.Now.Ticks;
        var changedBoard = await _boardClient.UpdateBoard(_createdBoardId, newBoardName);
        
        Assert.That(changedBoard.Name, Is.EqualTo(newBoardName), "Changed board name does not match");
        Assert.That(changedBoard.Id, Is.EqualTo(_createdBoardId), "Changed board id does not match");
        
        TestContext.WriteLine($"Board updated. New name: {changedBoard.Name}");
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
