using System.Text.Json.Serialization;

namespace Trello.Api.RestSharp.Tests.Models;

public class ListModel
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("idBoard")]
    public string IdBoard { get; set; }
}
