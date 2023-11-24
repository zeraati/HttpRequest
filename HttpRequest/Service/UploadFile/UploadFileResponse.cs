using System.Net;

namespace HttpRequest;
public class UploadFileResponse
{

    public ResponseMeta Meta { get; }
    public ResponseData Data { get; }
}

public class ResponseMeta
{
    public HttpStatusCode Code { get; set; }
}

public class ResponseData
{
    public Guid Id { get; set; }
    public string Path { get; set; }
}
