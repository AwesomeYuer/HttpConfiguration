using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Settings;

namespace HttpConfiguration.Controllers;

[ApiController]
[Route("[controller]")]
public class HttpConfigurationReaderController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly MiscSettings _miscSettingsOptionsValue;
    private readonly MiscSettings _miscSettingsOptionsSnapshotValue;
    private readonly MiscSettings _miscSettingsOptionsCurrentValue;
    
    public HttpConfigurationReaderController
                    (
                        IConfiguration configuration,
                        IOptions<MiscSettings> miscSettingsOptions,
                        IOptionsSnapshot<MiscSettings> miscSettingsOptionsSnapshot,
                        IOptionsMonitor<MiscSettings> miscSettingsOptionsMonitor,
                        string configurationUrl
                    )
    {
        // 由于测试用 settings.json 也在本 WebApi Server 下, 随意注入些东西既可
        // WebApi Server 就绪后
        // program.cs 可以延迟回调 configurationManager.AddJsonHttpGet(configurationUrl!);
        _ = configurationUrl;
        _configuration = configuration;
        _miscSettingsOptionsValue = miscSettingsOptions.Value;
        _miscSettingsOptionsSnapshotValue = miscSettingsOptionsSnapshot.Value;
        _miscSettingsOptionsCurrentValue = miscSettingsOptionsMonitor.CurrentValue;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] string keyPrefix = "misc_")
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
                                            x.Key.StartsWith("misc_", StringComparison.OrdinalIgnoreCase);
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
    [Route("misc")]
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
}