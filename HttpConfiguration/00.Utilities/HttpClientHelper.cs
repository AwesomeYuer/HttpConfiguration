namespace Microshaoft;

public static class HttpClientHelper
{
    public static async Task AsUrlHttpGetContentReadAsStreamAsync
                    (
                        this string @this
                        , Func<Stream, Task> onContentReadAsStreamProcessAsync
                    )
    { 
        using var httpClient = new HttpClient();
        var uri = new Uri(@this);
        using var httpResponseMessage = await httpClient.GetAsync(uri);
        using var stream = await httpResponseMessage.Content.ReadAsStreamAsync();
        await onContentReadAsStreamProcessAsync(stream);
        stream.Close();
    }
}
