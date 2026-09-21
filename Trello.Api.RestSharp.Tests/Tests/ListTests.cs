using System.Net;
using Trello.Api.RestSharp.Tests.ApiClients;

namespace Trello.Api.RestSharp.Tests;

public class ListTests : BaseTest
{
    private string _createdBoardId;
    private BoardClient _boardClient;
    private ListClient _listClient;

    [SetUp]
    public void InitClient()
    {
        _boardClient = new BoardClient(Client, Key, Token);
        _listClient = new ListClient(Client, Key, Token);
    }

    [Test]
    public async Task CreateList_Successfully()
    {
        string boardName = "AutoTest_Board_" + DateTime.Now.Ticks;
        var boardResponse = await _boardClient.CreateBoard(boardName);
        
        Assert.That(boardResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        _createdBoardId = boardResponse.Data.Id;
        
        string listName = "AutoTest_List_" + DateTime.Now.Ticks;
        var listResponse = await _listClient.CreateList(listName, _createdBoardId);
        
        Assert.That(listResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(listResponse.Data.Id, Is.Not.Null.And.Not.Empty, "Created list id can not be null or empty");
        Assert.That(listResponse.Data.Name, Is.EqualTo(listName), "Created list name does not match");
        Assert.That(listResponse.Data.IdBoard, Is.EqualTo(_createdBoardId), "List was created on the wrong board");
        
        TestContext.WriteLine($"List created. Name: {listResponse.Data.Name}, Id: {listResponse.Data.Id}, BoardId: {listResponse.Data.IdBoard}");
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
