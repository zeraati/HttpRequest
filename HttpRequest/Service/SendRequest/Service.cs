using System.Text;
using Newtonsoft.Json;
using Common.Interface;
using HttpRequest.Common;
using System.Net.Http.Headers;
using Newtonsoft.Json.Serialization;

namespace HttpRequest;
public partial class Service : IAddScoped
{
	private readonly HttpClient _httpClient;
	public Service(IHttpClientFactory httpClientFactory) => _httpClient = httpClientFactory.CreateClient();

    public async Task<T> SendRequest<T>(Request dto)
    {
        if (string.IsNullOrEmpty(dto.Token) == false)
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", dto.Token);
        if (dto.Headers != null && dto.Headers.Any())
        {
            foreach (var item in dto.Headers)
            {
                _httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
            }
        }
        var result = new HttpResponseMessage();

        if (dto.HttpMethod == HttpMethod.Get) result = await _httpClient.GetAsync(dto.ApiUrl);
        else if (dto.HttpMethod == HttpMethod.Delete) result = await _httpClient.DeleteAsync(dto.ApiUrl);
        else if (dto.HttpMethod == HttpMethod.Post || dto.HttpMethod == HttpMethod.Put)
        {
            var jsonSetting = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var param = JsonConvert.SerializeObject(dto.Parameter, jsonSetting);
            var content = new StringContent(param, Encoding.UTF8, "application/json");

            if (dto.HttpMethod == HttpMethod.Post) result = await _httpClient.PostAsync(dto.ApiUrl, content);
            else if (dto.HttpMethod == HttpMethod.Put) result = await _httpClient.PutAsync(dto.ApiUrl, content);
        }

        var response = await SetResponse<T>(result);
        return response;
    }

    private static async Task<T> SetResponse<T>(HttpResponseMessage httpResponse)
    {
        var response = await httpResponse.Content.ReadAsStringAsync();

        try
        {
            var result = JsonConvert.DeserializeObject<T>(response);

            var type = result.GetType();
            var statusCode = type.GetProperty(nameof(BaseResponse.StatusCode));
            statusCode?.SetValue(result, httpResponse.StatusCode, null);

            return result;
        }
        catch (Exception)//TODO لاگ ثبت شود
        {
            return default;
        }
    }
}
