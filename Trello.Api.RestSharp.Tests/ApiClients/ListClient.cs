using RestSharp;
using Trello.Api.RestSharp.Tests.Models;

namespace Trello.Api.RestSharp.Tests.ApiClients;

public class ListClient
{
    private readonly RestClient _client;
    private readonly string _key;
    private readonly string _token;

    public ListClient(RestClient client, string key,  string token)
    {
        _client = client;
        _key = key;
        _token = token;
    }

    public async Task<RestResponse<ListModel>> CreateList(string name, string idBoard, string customToken = null)
    {
        var request = new RestRequest("/1/lists", Method.Post);
        request.AddQueryParameter("name", name);
        request.AddQueryParameter("idBoard", idBoard);
        request.AddQueryParameter("key", _key);
        request.AddQueryParameter("token", customToken ?? _token);
        
        return await _client.ExecuteAsync<ListModel>(request);
    }

    public async Task<RestResponse<ListModel>> GetList(string listId)
    {
        var request = new RestRequest($"/1/lists/{listId}", Method.Get);
        request.AddQueryParameter("key", _key);
        request.AddQueryParameter("token", _token);
        
        return await _client.ExecuteAsync<ListModel>(request);
    }

    public async Task<RestResponse<ListModel>> UpdateList(string listId, string newName, string customToken = null)
    {
        var request = new RestRequest($"/1/lists/{listId}", Method.Put);
        request.AddQueryParameter("name", newName);
        request.AddQueryParameter("key", _key);
        request.AddQueryParameter("token", customToken ?? _token);
        
        return await _client.ExecuteAsync<ListModel>(request);
    }
}
