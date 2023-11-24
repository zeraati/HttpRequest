using Newtonsoft.Json;
using Common.Interface;

namespace HttpRequest;
public partial class Service : IAddScoped
{
    public async Task<string?> UploadFile(UploadFileRequest request)
    {
        var data = new MultipartFormDataContent();
        await using (var memoryStream = new MemoryStream(request.File))
        {
            var a = memoryStream.ToArray();
            data.Add(new ByteArrayContent(memoryStream.ToArray()), "file", "QRCodeImage");
        }

        //TODO حتما از ستینگ خوانده شود
        var url = "https://blobe.ex-pilot.ir/Blob/Upload";
        var responseMessage = await _httpClient.PostAsync(url, data);
        var response = await responseMessage.Content.ReadAsStringAsync();

        try
        {
            var result = JsonConvert.DeserializeObject<UploadFileResponse>(response);
            return result?.Data?.Path;
        }
        catch (Exception)//TODO لاگ ثبت شود
        {
            return default;
        }
    }
}
