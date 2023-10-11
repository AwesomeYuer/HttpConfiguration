using Microshaoft;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Settings;

namespace HttpConfiguration.Controllers;

[ApiController]
[Route("[controller]")]
public class HttpConfigurationReloaderController : ControllerBase
{
    private readonly ConfigurationManager _configurationManager;
    private readonly string _configurationUrl;

    public HttpConfigurationReloaderController
                    (
                        //ILogger<HttpConfigurationReloaderController> logger,
                        //IConfiguration configuration,
                        ConfigurationManager configurationManager,
                        string configurationUrl
                    )
    {
        //_logger = logger;
        //_configuration = configuration;
        _configurationManager = configurationManager;
        _configurationUrl = configurationUrl;
    }

    [HttpPost]
    public async Task<IActionResult> RefreshAsync()
    {
        using (var stream = await _configurationUrl.AsUrlHttpGetContentReadAsStreamAsync())
        {
            _configurationManager
                            .AddJsonStream(stream);
        }
        return
            await Task.FromResult(Ok());
             
    }
}