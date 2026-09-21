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
    
    [Test]
    public async Task GetList_Successfully()
    {
        string boardName = "AutoTest_Board_" + DateTime.Now.Ticks;
        var boardResponse = await _boardClient.CreateBoard(boardName);
        
        Assert.That(boardResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        _createdBoardId = boardResponse.Data.Id;
        
        string listName = "AutoTest_List_" + DateTime.Now.Ticks;
        var listResponse = await _listClient.CreateList(listName, _createdBoardId);
        
        Assert.That(listResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        var retrievedList = await _listClient.GetList(listResponse.Data.Id);
        
        Assert.That(retrievedList.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(retrievedList.Data.Id, Is.EqualTo(listResponse.Data.Id), "Retrieved list id does not match");
        Assert.That(retrievedList.Data.Name, Is.EqualTo(listName), "Retrieved list name does not match");
        
        TestContext.WriteLine($"List {retrievedList.Data.Name} found");
    }

    [Test]
    public async Task UpdateList_Successfully()
    {
        string boardName = "AutoTest_Board_" + DateTime.Now.Ticks;
        var boardResponse = await _boardClient.CreateBoard(boardName);
        
        Assert.That(boardResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        _createdBoardId = boardResponse.Data.Id;
        
        string listName = "AutoTest_List_" + DateTime.Now.Ticks;
        var initialListResponse = await _listClient.CreateList(listName, _createdBoardId);
        
        string newListName = "AutoTest_List_Changed_" + DateTime.Now.Ticks;
        var changedListResponse = await _listClient.UpdateList(initialListResponse.Data.Id, newListName);
        
        Assert.That(changedListResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(changedListResponse.Data.Id, Is.EqualTo(initialListResponse.Data.Id), "Changed list id does not match");
        Assert.That(changedListResponse.Data.Name, Is.EqualTo(newListName), "Changed list name does not match");
        
        TestContext.WriteLine($"List updated. New name: {changedListResponse.Data.Name}");
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
