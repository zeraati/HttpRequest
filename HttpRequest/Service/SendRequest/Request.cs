namespace HttpRequest;
public class Request
{
    public Request(HttpMethod httpMethod, string apiUrl)
    {
        HttpMethod = httpMethod;
        ApiUrl = apiUrl;
    }

    public Request(HttpMethod httpMethod, string apiUrl, string token) : this(httpMethod, apiUrl)
    {
        Token = token;
    }

    public Request(HttpMethod httpMethod, string apiUrl, object parameter) : this(httpMethod, apiUrl)
    {
        Parameter = parameter;
    }

    public Request(HttpMethod httpMethod, string apiUrl, object parameter, string token) : this(httpMethod, apiUrl, parameter)
    {
        HttpMethod = httpMethod;
        ApiUrl = apiUrl;
        Parameter = parameter;
        Token = token;
    }

    public Request(HttpMethod httpMethod, string apiUrl, object parameter, string token, Dictionary<string, string> headers) : this(httpMethod, apiUrl, parameter, token)
    {
        HttpMethod = httpMethod;
        ApiUrl = apiUrl;
        Parameter = parameter;
        Token = token;
        Headers = headers;
    }

    public void SetToken(string token) => Token = token;
    public void SetBaseUrl(string baseUrl) => ApiUrl = baseUrl + ApiUrl;
    public void SetHeaders(Dictionary<string, string> headers) => Headers = headers;
    public HttpMethod HttpMethod { get; }
    public string ApiUrl { get; private set; }
    public object? Parameter { get; private set; }
    public string? Token { get; private set; }
    public Dictionary<string, string> Headers { get; private set; }
}
