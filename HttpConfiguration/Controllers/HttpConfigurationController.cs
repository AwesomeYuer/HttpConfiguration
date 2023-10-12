using Microshaoft;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Settings;

namespace HttpConfiguration.Controllers;

[ApiController]
[Route("[controller]")]
public class HttpConfigurationController : ControllerBase
{
    private readonly IConfiguration _configuration;

    private readonly MiscSettings _miscSettingsOptionsValue;
    private readonly MiscSettings _miscSettingsOptionsSnapshotValue;
    private readonly MiscSettings _miscSettingsOptionsCurrentValue;

    private readonly string _configurationUrl;

    public HttpConfigurationController
                    (
                        string configurationUrl,

                        IConfiguration configuration,

                        IOptions<MiscSettings> miscSettingsOptions,
                        IOptionsSnapshot<MiscSettings> miscSettingsOptionsSnapshot,
                        IOptionsMonitor<MiscSettings> miscSettingsOptionsMonitor
                    )
    {
        _configurationUrl = configurationUrl;

        _configuration = configuration;
                
        _miscSettingsOptionsValue = miscSettingsOptions.Value;
        _miscSettingsOptionsSnapshotValue = miscSettingsOptionsSnapshot.Value;
        _miscSettingsOptionsCurrentValue = miscSettingsOptionsMonitor.CurrentValue;
    }

    [HttpGet]
    [Route("read")]
    public async Task<IActionResult> GetAsync([FromQuery] string keyPrefix = "misc")
    {
        var keyValuePairs =
                    _configuration
                            .AsEnumerable();

        if 
            (
                !string.IsNullOrEmpty(keyPrefix)
                &&
                keyPrefix != "*"
            )
        {
            keyValuePairs =
                    keyValuePairs
                               .Where
                                (
                                    (x) =>
                                    {
                                        return
                                            x
                                                .Key
                                                .StartsWith
                                                    (
                                                        keyPrefix
                                                        , StringComparison
                                                                .OrdinalIgnoreCase
                                                    );
                                    }
                                )
                            ;
        }
        return
            await
                Task
                    .FromResult
                        (
                            Ok(keyValuePairs)
                        );
    }

    [HttpGet]
    [Route("read/MiscSettings")]
    public async Task<IActionResult> GetMiscSettingsAsync()
    {
        return
            await
                Task
                    .FromResult
                        (
                            Ok
                                (
                                    new
                                    { 
                                        OptionsValue = new 
                                        { 
                                            misc = _miscSettingsOptionsValue
                                        },
                                        OptionsSnapshotValue = new
                                        {
                                            misc = _miscSettingsOptionsSnapshotValue
                                        },
                                        OptionsCurrentValue = new
                                        {
                                            misc = _miscSettingsOptionsCurrentValue
                                        }
                                    }
                                )
                        );
    }

    [HttpPost]
    [Route("refresh")]
    public async Task<IActionResult> RefreshAsync()
    {
        var configurationBuilder = (IConfigurationBuilder) _configuration; 

        configurationBuilder.AddJsonHttpGet(_configurationUrl);

        var result = await GetAsync(keyPrefix: "misc");

        return result;
    }
}