namespace HttpRequest;
public class UploadFileRequest
{
    public UploadFileRequest(byte[] file) => File = file;
    public UploadFileRequest(string file)=> File = Convert.FromBase64String(file);

    public byte[] File { get; }
}
