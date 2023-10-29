using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace HttpConfiguration.Controllers;

[ApiController]
[Route("[controller]")]
public class EnvironmentVariablesController : ControllerBase
{
    
    [HttpGet]
    [Route("read")]
    public async Task<IActionResult> GetAsync([FromQuery] string keyPrefix = "var")
    {
        IEnumerable
            <
                (
                    string EnvironmentVariableName
                    , string EnvironmentVariableValue
                )
            >
                GetEnvironmentVariablesAsIEnumerable()
        {
            var environmentVariables = Environment.GetEnvironmentVariables();
            foreach (DictionaryEntry dictionaryEntry in environmentVariables)
            {
                yield return
                        (
                            (string) dictionaryEntry.Key
                            , (string) dictionaryEntry.Value!
                        );
            }
        }

        return
            await
                Task
                    .FromResult
                        (
                            Ok
                                (
                                    GetEnvironmentVariablesAsIEnumerable()
                                        .Where
                                            (
                                                (x) =>
                                                {
                                                    return
                                                        x
                                                            .EnvironmentVariableName
                                                            .StartsWith(keyPrefix, StringComparison.OrdinalIgnoreCase);
                                                }
                                            )
                                        .Select
                                            (
                                                (x) =>
                                                {
                                                    return
                                                            new
                                                            {
                                                                x.EnvironmentVariableName,
                                                                x.EnvironmentVariableValue,
                                                                GetEnvironmentVariable =
                                                                                    Environment
                                                                                        .GetEnvironmentVariable
                                                                                            (x.EnvironmentVariableName)
                                                            };
                                                }
                                            )
                                )
                        );
    }
}