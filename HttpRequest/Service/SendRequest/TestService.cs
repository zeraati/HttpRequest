using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Newtonsoft.Json.Serialization;

namespace HttpRequest;
public class TestService
{
	private readonly string _baseUrl;
    public TestService(string baseUrl)=>_baseUrl=baseUrl;

	public async Task<T> SendRequest<T>(Request dto)
	{
		var httpClient=new HttpClient();

		dto.SetBaseUrl(_baseUrl);

		if (string.IsNullOrEmpty(dto.Token) == false)
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", dto.Token);

		var result = new HttpResponseMessage();

		if (dto.HttpMethod == HttpMethod.Get) result = await httpClient.GetAsync(dto.ApiUrl.ToLower());
		else if (dto.HttpMethod == HttpMethod.Delete) result = await httpClient.DeleteAsync(dto.ApiUrl.ToLower());
		else if (dto.HttpMethod == HttpMethod.Post || dto.HttpMethod == HttpMethod.Put)
		{
			var jsonSetting = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
			var param = JsonConvert.SerializeObject(dto.Parameter, jsonSetting);
			var content = new StringContent(param, Encoding.UTF8, "application/json");

			if (dto.HttpMethod == HttpMethod.Post) result = await httpClient.PostAsync(dto.ApiUrl, content);
			else if (dto.HttpMethod == HttpMethod.Put) result = await httpClient.PutAsync(dto.ApiUrl, content);
		}

		var response = await SetResponse<T>(result);
		return response;
	}

	private static async Task<T> SetResponse<T>(HttpResponseMessage httpResponse)
	{
		var response = await httpResponse.Content.ReadAsStringAsync();
		var result = JsonConvert.DeserializeObject<T>(response);

		if (result != null)
		{
			var type = result.GetType();
			var statusCode = type.GetProperty("StatusCode");
			statusCode?.SetValue(result, httpResponse.StatusCode, null);

			return result;
		}

		return default!;
	}
}
