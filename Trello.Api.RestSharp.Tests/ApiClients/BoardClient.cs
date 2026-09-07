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

    public async Task<BoardModel> CreateBoard(string name)
    {
        var request = new RestRequest("/1/boards", Method.Post);
        request.AddQueryParameter("name", name);
        request.AddQueryParameter("key", _key);
        request.AddQueryParameter("token", _token);
        
        var response = await _client.ExecuteAsync<BoardModel>(request);
        return response.Data;
    }

    public async Task<BoardModel> GetBoard(string boardId)
    {
        var request = new RestRequest($"/1/boards/{boardId}", Method.Get);
        request.AddQueryParameter("key", _key);
        request.AddQueryParameter("token", _token);
        
        var response = await _client.ExecuteAsync<BoardModel>(request);
        return response.Data;
    }

    public async Task<System.Net.HttpStatusCode> DeleteBoard(string boardId)
    {
        var request = new RestRequest($"1/boards/{boardId}", Method.Delete);
        request.AddQueryParameter("key",  _key);
        request.AddQueryParameter("token",  _token);
        
        var response = await _client.ExecuteAsync(request);
        return response.StatusCode;
    }
}
