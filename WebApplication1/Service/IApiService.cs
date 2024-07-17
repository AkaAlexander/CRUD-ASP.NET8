using WebApplication1.Models;

namespace WebApplication1.Service
{
	public interface IApiService
	{
		Task<List<Usuario>> FetchUsersFromApi(string apiUrl);
	}

}
