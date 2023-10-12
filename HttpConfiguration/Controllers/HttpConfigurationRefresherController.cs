using Microshaoft;
using Microsoft.AspNetCore.Mvc;

namespace HttpConfiguration.Controllers;

[ApiController]
[Route("[controller]")]
public class HttpConfigurationRefresherController : ControllerBase
{
    private readonly IConfigurationBuilder _configurationBuilder;
    private readonly string _configurationUrl;

    public HttpConfigurationRefresherController
                    (
                        IConfigurationBuilder configurationBuilder,
                        string configurationUrl
                    )
    {
        _configurationBuilder = configurationBuilder;
        _configurationUrl = configurationUrl;
    }

    [HttpPost]
    public async Task<IActionResult> RefreshAsync()
    {
        _configurationBuilder.AddJsonHttpGet(_configurationUrl);
        return
            await Task.FromResult(Ok());
    }
}