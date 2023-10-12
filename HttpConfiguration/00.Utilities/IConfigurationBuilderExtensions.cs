namespace Microshaoft;

public static class IConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AddJsonHttpGet
                    (
                        this IConfigurationBuilder @this
                        , string jsonFileUrl
                    )
    {
        jsonFileUrl
                    .AsUrlHttpGetContentReadAsStreamAsync
                        (
                            async (stream) =>
                            {
                                @this.AddJsonStream(stream);
                                await Task.CompletedTask;
                            }
                        )
                    .Wait();
        return @this;
    }
}
