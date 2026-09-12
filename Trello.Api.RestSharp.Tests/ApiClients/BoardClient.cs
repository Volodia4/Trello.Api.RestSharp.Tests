using RestSharp;
using Trello.Api.RestSharp.Tests.Models;

namespace Trello.Api.RestSharp.Tests.ApiClients;

public class BoardClient
{
    private readonly RestClient _client;
    private readonly string _key;
    private readonly string _token;
    
    public BoardClient(RestClient client, string key, string token)
    {
        _client = client;
        _key = key;
        _token = token;
    }

    public async Task<RestResponse<BoardModel>> CreateBoard(string name, string customToken = null)
    {
        var request = new RestRequest("/1/boards", Method.Post);
        request.AddQueryParameter("name", name);
        request.AddQueryParameter("key", _key);
        request.AddQueryParameter("token", customToken ?? _token);
        
        return await _client.ExecuteAsync<BoardModel>(request);
    }

    public async Task<RestResponse<BoardModel>> GetBoard(string boardId)
    {
        var request = new RestRequest($"/1/boards/{boardId}", Method.Get);
        request.AddQueryParameter("key", _key);
        request.AddQueryParameter("token", _token);
        
        return await _client.ExecuteAsync<BoardModel>(request);
    }

    public async Task<RestResponse<BoardModel>> UpdateBoard(string boardId, string newName, string customToken = null)
    {
        var request = new RestRequest($"/1/boards/{boardId}", Method.Put);
        request.AddQueryParameter("name", newName);
        request.AddQueryParameter("key", _key);
        request.AddQueryParameter("token", customToken ?? _token);
        
        return await _client.ExecuteAsync<BoardModel>(request);
    }

    public async Task<RestResponse> DeleteBoard(string boardId, string customToken = null)
    {
        var request = new RestRequest($"/1/boards/{boardId}", Method.Delete);
        request.AddQueryParameter("key", _key);
        request.AddQueryParameter("token", customToken ?? _token);
        
        return await _client.ExecuteAsync(request);
    }
}
