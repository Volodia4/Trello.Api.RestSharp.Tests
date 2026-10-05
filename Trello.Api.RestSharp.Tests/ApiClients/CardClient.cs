using RestSharp;
using Trello.Api.RestSharp.Tests.Models;

namespace Trello.Api.RestSharp.Tests.ApiClients;

public class CardClient
{
    private readonly RestClient _client;
    private readonly string _key;
    private readonly string _token;

    public CardClient(RestClient client, string key,  string token)
    {
        _client = client;
        _key = key;
        _token = token;
    }

    public async Task<RestResponse<CardModel>> CreateCard(string name, string idList, string customToken = null)
    {
        var request = new RestRequest("/1/cards", Method.Post);
        request.AddQueryParameter("name", name);
        request.AddQueryParameter("idList", idList);
        request.AddQueryParameter("key", _key);
        request.AddQueryParameter("token", customToken ?? _token);
        
        return await _client.ExecuteAsync<CardModel>(request);
    }

    public async Task<RestResponse<CardModel>> GetCard(string cardId)
    {
        var request = new RestRequest($"/1/cards/{cardId}", Method.Get);
        request.AddQueryParameter("key", _key);
        request.AddQueryParameter("token", _token);
        
        return await _client.ExecuteAsync<CardModel>(request);
    }

    public async Task<RestResponse<CardModel>> UpdateCard(string cardId, string newName, string desc = null, string customToken = null)
    {
        var request = new RestRequest($"/1/cards/{cardId}", Method.Put);
        request.AddQueryParameter("name", newName);
        
        if (desc != null) request.AddQueryParameter("desc", desc);
        
        request.AddQueryParameter("key", _key);
        request.AddQueryParameter("token", customToken ?? _token);
        
        return await _client.ExecuteAsync<CardModel>(request);
    }
}
