using Microshaoft;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HttpConfiguration.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HttpConfigurationController : ControllerBase
    {
        private readonly ILogger<HttpConfigurationController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ConfigurationManager _configurationManager;
        private readonly string _configurationUrl;

        public HttpConfigurationController
                        (
                            ILogger<HttpConfigurationController> logger,
                            IConfiguration configuration,
                            ConfigurationManager configurationManager,
                            string configurationUrl
                        )
        {
            _logger = logger;
            _configuration = configuration;
            _configurationManager = configurationManager;
            _configurationUrl = configurationUrl;
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


        [HttpPost]
        public async Task<IActionResult> RefreshAsync()
        {
            using (var stream = await _configurationUrl.AsUrlHttpGetContentReadAsStreamAsync())
            {
                _configurationManager
                                .AddJsonStream(stream);
                                //.Build();
            }
            return
                await GetAsync("*");
                 
        }
    }
}