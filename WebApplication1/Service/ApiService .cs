using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using WebApplication1.Models;

namespace WebApplication1.Service
{
	public class ApiService : IApiService
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public ApiService(IHttpClientFactory httpClientFactory) 
		{
			_httpClientFactory = httpClientFactory;
		}

		public async Task<List<Usuario>> FetchUsersFromApi(string apiUrl)
		{
			var client = _httpClientFactory.CreateClient();
			HttpResponseMessage response = await client.GetAsync(apiUrl);
			response.EnsureSuccessStatusCode();
			string responseBody = await response.Content.ReadAsStringAsync();
			var users = JsonConvert.DeserializeObject<List<Usuario>>(responseBody);

			return users ?? throw new Exception("Los datos no son validos");

		}
	}
}
