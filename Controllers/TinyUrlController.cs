using Microsoft.AspNetCore.Mvc;
using TinyURL.Services;

namespace TinyURL.Controllers;

[ApiController]
[Route("tiny")]
public class TinyUrlController : ControllerBase
{
    private readonly TinyUrlService _tinyUrlService;

    public TinyUrlController(TinyUrlService tinyUrlService)
    {
        _tinyUrlService = tinyUrlService;
    }

    [HttpPost]
    public async Task<ActionResult<string>> GetTinyUrl([FromBody] string urlInput)
    {
        if (!Uri.TryCreate(urlInput, UriKind.Absolute, out _))
        {
            return BadRequest("Url is not valid");
        }

        return await _tinyUrlService.CreateShortUrl(urlInput);
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> RedirectShortUrl(string code)
    {
        try
        {
            var longUrl = await _tinyUrlService.GetShortUrlByCode(code);
            return Redirect(longUrl);
        }
        catch (Exception e)
        {
            await Response.WriteAsync(e.Message);
            return StatusCode(400);
        }
    }
}