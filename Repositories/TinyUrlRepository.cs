using MongoDB.Driver;
using TinyURL.Extenstions;
using TinyURL.Models;

namespace TinyURL.Repositories;

public class TinyUrlRepository
{
    private readonly IMongoCollection<TinyUrl> _collection;
    private readonly ILogger<TinyUrlRepository> _logger;

    public TinyUrlRepository(IMongoClient mongoClient, IConfiguration configuration)
    {
        var mongoDatabase = mongoClient.GetDatabase(configuration.MongoDbName());
        _collection = mongoDatabase.GetCollection<TinyUrl>(configuration.MongoConnectionString());
    }
    
    public async Task AddItemAsync(TinyUrl tinyUrl)
    {
        try
        {
            await _collection.InsertOneAsync(tinyUrl);
        }
        catch (MongoException e)
        {
            _logger.Log(LogLevel.Error, "Failed to save item to db");
        }
    }
    
    public async Task<TinyUrl?> GetTinyUrlByCode(string code)
    {
        var filter = Builders<TinyUrl>.Filter.Eq(doc => doc.Code, code);
        var tinyUrl = await _collection.Find(filter).FirstOrDefaultAsync();
        return tinyUrl;
    }
}