using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PhoneInfo.API.Core.HttpRequest
{
	public interface IHttpService
	{
		Task<(T response, HttpStatusCode statusCode)> SendRequestWithStringContentAsync<T>(HttpMethod method, string endpoint, string body = null, Dictionary<string, string> headers = null) where T : class;

		Task<(T, HttpStatusCode)> SendRequestWithFormDataContentAsync<T>(HttpMethod method, string endpoint, Dictionary<string, string> datas = null, Dictionary<string, string> headers = null) where T : class;
	}

	public class HttpService : IHttpService
	{
		private readonly HttpClient _httpClient;

		public HttpService()
		{
			_httpClient = new();
		}

		public async Task<(T, HttpStatusCode)> SendRequestWithStringContentAsync<T>
			(HttpMethod method, string endpoint, string body = null, Dictionary<string, string> headers = null) where T : class
		{
			HttpRequestMessage requestMessage = new(method, endpoint);

			if (headers is not null)
			{
				foreach (KeyValuePair<string, string> header in headers)
				{
					requestMessage.Headers.Add(header.Key, header.Value);
				}
			}
			if (!string.IsNullOrEmpty(body))
			{
				StringContent content = new(body);
				requestMessage.Content = content;
			}

			return await ProcessingHttpResponse<T>(requestMessage);
		}

		public async Task<(T, HttpStatusCode)> SendRequestWithFormDataContentAsync<T>
			(HttpMethod method, string endpoint, Dictionary<string, string> datas = null, Dictionary<string, string> headers = null) where T : class
		{
			HttpRequestMessage requestMessage = new(method, endpoint);

			if (headers is not null)
			{
				foreach (KeyValuePair<string, string> header in headers)
				{
					requestMessage.Headers.Add(header.Key, header.Value);
				}
			}

			if (datas is not null)
			{
				MultipartFormDataContent content = new();
				foreach (KeyValuePair<string, string> data in datas)
				{
					content.Add(new StringContent(data.Value), data.Key);
				}
				requestMessage.Content = content;
			}

			return await ProcessingHttpResponse<T>(requestMessage);
		}

		private async Task<(T, HttpStatusCode)> ProcessingHttpResponse<T>(HttpRequestMessage requestMessage)
		{
			HttpResponseMessage responseMessage = await _httpClient.SendAsync(requestMessage);

			T responseObj = default;
			if (responseMessage.IsSuccessStatusCode)
			{
				string responseAsString = await responseMessage.Content.ReadAsStringAsync();
				responseObj = typeof(T) == typeof(string) ? (T)(object)responseAsString : JsonConvert.DeserializeObject<T>(responseAsString);
			}
			return (responseObj, responseMessage.StatusCode);
		}
	}
}