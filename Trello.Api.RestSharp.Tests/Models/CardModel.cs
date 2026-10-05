using System.Text.Json.Serialization;

namespace Trello.Api.RestSharp.Tests.Models;

public class CardModel
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("idList")]
    public string IdList { get; set; }
    
    [JsonPropertyName("idBoard")]
    public string IdBoard { get; set; }
    
    [JsonPropertyName("desc")]
    public string Desc { get; set; }
}
