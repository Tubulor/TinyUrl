using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ThirdParty.Json.LitJson;

namespace TinyURL.Models;

public class TinyUrl
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Code { get; set; }
    public string LongUrl { get; set; }
    public string ShortUrl { get; set; }
}