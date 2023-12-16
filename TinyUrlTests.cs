using Microsoft.AspNetCore.Mvc.Testing;
using TinyURL.Services;
using Xunit;

namespace TinyURL;

public class TinyUrlTests: IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public TinyUrlTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    
    [Fact]
    // Add more test cases as needed
    public async Task ConcurrentAccessTest()
    {
        var scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();

        using var scope = scopeFactory.CreateScope();
        var tinyUrlService = scope.ServiceProvider.GetRequiredService<TinyUrlService>();
        var shortUrl = await tinyUrlService.CreateShortUrl("https://google.com");
        var code = shortUrl.Substring(shortUrl.LastIndexOf('/') + 1);
        
        // Use Parallelize attribute to run tests concurrently
        Parallel.For(0, 3000, async i =>
        {
            // Execute the method under test concurrently
            var result = tinyUrlService.GetShortUrlByCode(code);
            
            // Add assertions based on the expected behavior of GetShortUrlByCode
            Assert.NotNull(result);
        });
    }
    
}