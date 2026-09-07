using System.Text.Json.Serialization;

namespace Trello.Api.RestSharp.Tests.Models;

public class BoardModel
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
}
