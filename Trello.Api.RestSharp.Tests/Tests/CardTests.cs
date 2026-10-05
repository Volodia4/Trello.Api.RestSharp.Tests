using System.Net;
using Trello.Api.RestSharp.Tests.ApiClients;

namespace Trello.Api.RestSharp.Tests;

public class CardTests : BaseTest
{
    private string _createdBoardId;
    private BoardClient _boardClient;
    private ListClient _listClient;
    private CardClient _cardClient;

    [SetUp]
    public void InitClient()
    {
        _boardClient = new BoardClient(Client, Key, Token);
        _listClient = new ListClient(Client, Key, Token);
        _cardClient = new CardClient(Client, Key, Token);
    }

    [Test]
    public async Task CreateCard_Successfully()
    {
        string boardName = "AutoTest_Board_" + DateTime.Now.Ticks;
        var boardResponse = await _boardClient.CreateBoard(boardName);
        Assert.That(boardResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        _createdBoardId = boardResponse.Data.Id;
        
        string listName = "AutoTest_List_" + DateTime.Now.Ticks;
        var listResponse = await _listClient.CreateList(listName, _createdBoardId);
        Assert.That(listResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        string cardName = "AutoTest_Card_" + DateTime.Now.Ticks;
        var cardResponse = await _cardClient.CreateCard(cardName, listResponse.Data.Id);
        
        Assert.That(cardResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(cardResponse.Data.Id, Is.Not.Null.And.Not.Empty, "Created card id can not be null or empty");
        Assert.That(cardResponse.Data.Name, Is.EqualTo(cardName), "Created card name does not match");
        Assert.That(cardResponse.Data.IdList, Is.EqualTo(listResponse.Data.Id), "Card was created on the wrong list");
        Assert.That(cardResponse.Data.IdBoard, Is.EqualTo(_createdBoardId), "Card was created on the wrong board");
        
        TestContext.WriteLine($"Card created. Name: {cardResponse.Data.Name}, Id: {cardResponse.Data.Id}, " +
                              $"ListId: {cardResponse.Data.IdList} BoardId: {cardResponse.Data.IdBoard}");
    }

    [Test]
    public async Task GetCard_Successfully()
    {
        string boardName = "AutoTest_Board_" + DateTime.Now.Ticks;
        var boardResponse = await _boardClient.CreateBoard(boardName);
        Assert.That(boardResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        _createdBoardId = boardResponse.Data.Id;
        
        string listName = "AutoTest_List_" + DateTime.Now.Ticks;
        var listResponse = await _listClient.CreateList(listName, _createdBoardId);
        Assert.That(listResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        string cardName = "AutoTest_Card_" + DateTime.Now.Ticks;
        var cardResponse = await _cardClient.CreateCard(cardName, listResponse.Data.Id);
        Assert.That(cardResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var retrievedCard = await _cardClient.GetCard(cardResponse.Data.Id);
        
        Assert.That(retrievedCard.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(retrievedCard.Data.Id, Is.EqualTo(cardResponse.Data.Id), "Retrieved card id does not match");
        Assert.That(retrievedCard.Data.Name, Is.EqualTo(cardName), "Retrieved card name does not match");
        
        TestContext.WriteLine($"Card {retrievedCard.Data.Name} found");
    }

    [Test]
    public async Task UpdateCard_Successfully()
    {
        string boardName = "AutoTest_Board_" + DateTime.Now.Ticks;
        var boardResponse = await _boardClient.CreateBoard(boardName);
        Assert.That(boardResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        _createdBoardId = boardResponse.Data.Id;
        
        string listName = "AutoTest_List_" + DateTime.Now.Ticks;
        var listResponse = await _listClient.CreateList(listName, _createdBoardId);
        Assert.That(listResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        string cardName = "AutoTest_Card_" + DateTime.Now.Ticks;
        var initialCardResponse = await _cardClient.CreateCard(cardName, listResponse.Data.Id);
        Assert.That(initialCardResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        string newCardName = "AutoTest_Card_Changed_" + DateTime.Now.Ticks;
        string newDescription = "AutoTest description";
        
        var changedCardResponse = await _cardClient.UpdateCard(initialCardResponse.Data.Id, newCardName, newDescription);
        
        Assert.That(changedCardResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(changedCardResponse.Data.Id, Is.EqualTo(initialCardResponse.Data.Id), "Changed card id does not match");
        Assert.That(changedCardResponse.Data.Name, Is.EqualTo(newCardName), "Changed card name does not match");
        Assert.That(changedCardResponse.Data.Desc, Is.EqualTo(newDescription), "Card description was not updated correctly");
        
        TestContext.WriteLine($"Card updated. New name: {changedCardResponse.Data.Name}, Desc: {changedCardResponse.Data.Desc}");
    }

    [Test]
    public async Task CreateCard_WrongToken()
    {
        string boardName = "AutoTest_Board_" + DateTime.Now.Ticks;
        var boardResponse = await _boardClient.CreateBoard(boardName);
        Assert.That(boardResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        _createdBoardId = boardResponse.Data.Id;
        
        string listName = "AutoTest_List_" + DateTime.Now.Ticks;
        var listResponse = await _listClient.CreateList(listName, _createdBoardId);
        Assert.That(listResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        string cardName = "AutoTest_Card_WrongToken_" + DateTime.Now.Ticks;
        var cardResponse = await _cardClient.CreateCard(cardName, listResponse.Data.Id, "invalid_token_123");
        
        Assert.That(cardResponse.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        
        TestContext.WriteLine($"Success. Card with wrong token was not created");
    }

    [Test]
    public async Task CreateCard_WrongListId()
    {
        string boardName = "AutoTest_Board_" + DateTime.Now.Ticks;
        var boardResponse = await _boardClient.CreateBoard(boardName);
        Assert.That(boardResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        _createdBoardId = boardResponse.Data.Id;
        
        string cardName = "AutoTest_Card_" + DateTime.Now.Ticks;
        var cardResponse = await _cardClient.CreateCard(cardName, "123456789012345678901234");
        
        Assert.That(cardResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        
        TestContext.WriteLine($"Success. Card with wrong list id was not created");
    }

    [Test]
    public async Task GetCard_WrongId()
    {
        var response = await _cardClient.GetCard("123456789012345678901234");
        
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        
        TestContext.WriteLine("Success. Card with wrong id was not found");
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
