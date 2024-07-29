using Newtonsoft.Json;
using WebApplication1.Models;

namespace WebApplication1.Service
{
    public class ApiService : IApiService
	{
		private readonly IHttpClientFactory _httpClientFactory;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="httpClientFactory"></param>
		public ApiService(IHttpClientFactory httpClientFactory) 
		{
			_httpClientFactory = httpClientFactory;
		}

        /// <summary>
        /// Metodo que realiza una llamada a la api para obtener los usuarios
        /// </summary>
        /// <returns>Listado de usuarios</returns>
        /// <exception cref="Exception"></exception>
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
