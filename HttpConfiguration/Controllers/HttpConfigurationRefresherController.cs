using Microshaoft;
using Microsoft.AspNetCore.Mvc;

namespace HttpConfiguration.Controllers;

[ApiController]
[Route("[controller]")]
public class HttpConfigurationRefresherController : ControllerBase
{
    private readonly ConfigurationManager _configurationManager;
    private readonly string _configurationUrl;

    public HttpConfigurationRefresherController
                    (
                        ConfigurationManager configurationManager,
                        string configurationUrl
                    )
    {
        _configurationManager = configurationManager;
        _configurationUrl = configurationUrl;
    }

    [HttpPost]
    public async Task<IActionResult> RefreshAsync()
    {
        await _configurationUrl
                        .AsUrlHttpGetContentReadAsStreamAsync
                            (
                                async (stream) =>
                                {
                                    _configurationManager.AddJsonStream(stream);
                                    await Task.CompletedTask;
                                }
                            );
        return
            await Task.FromResult(Ok());
    }
}