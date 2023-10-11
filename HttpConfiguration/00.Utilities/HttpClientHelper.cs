namespace Microshaoft;

public static class HttpClientHelper
{
    public static async Task<Stream> AsUrlHttpGetContentReadAsStreamAsync(this string @this)
    { 
        using var httpClient = new HttpClient();
        var uri = new Uri(@this);
        using var response = httpClient.GetAsync(uri);
        return await response.Result.Content.ReadAsStreamAsync();
    }
}
