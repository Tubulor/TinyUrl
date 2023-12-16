using System.Security.Cryptography;
using System.Text;
using TinyURL.Cache;
using TinyURL.Extenstions;
using TinyURL.Models;
using TinyURL.Repositories;

namespace TinyURL.Services;

public class TinyUrlService
{

    private readonly TinyUrlRepository _repository;
    private readonly IConfiguration _configuration;
    private readonly LRUCache<string, string> _cache;
    private readonly SemaphoreSlim _semaphore = new(initialCount: 1, maxCount: 1);


    public TinyUrlService(TinyUrlRepository repository, IConfiguration configuration)
    {
        _configuration = configuration;
        _repository = repository;
        _cache = new LRUCache<string, string>(maxCapacity: 10);
    }

    public async Task<string> CreateShortUrl(string longUrl)
    {
        var code = GenerateShortUrlCode(longUrl);
        var tinyUrl = new TinyUrl
        {
            Code = code,
            LongUrl = longUrl,
            ShortUrl = $"{_configuration.HostUrl()}/tiny/{code}"
        };
        await _repository.AddItemAsync(tinyUrl);
        return tinyUrl.ShortUrl;
    }

    private string GenerateShortUrlCode(string longUrl)
    {
        // sha256 is deterministic algorithm, also it's almost unlikely he will give the same results for different long urls.
        // it's also possible to use md5 for this but he has higher odds to get the same results for different long urls.
        using SHA256 sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(longUrl));

        // Remove special chars 
        var shortUrl = BitConverter.ToString(hashBytes)
            .Replace("-", "")[..8];

        return shortUrl;
    }

    public async Task<string> GetShortUrlByCode(string code)
    {
        if (_cache.TryGetValue(code, out var url)) return url!;
        await _semaphore.WaitAsync();
        if (_cache.TryGetValue(code, out var tryAgainUrl)) return url!;
        var tinyUrl = await _repository.GetTinyUrlByCode(code);
        _cache.Set(code, tinyUrl.LongUrl);
        _semaphore.Release();
        return tinyUrl.LongUrl;
    }
}