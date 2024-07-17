using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Service;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
	private readonly IApiService _apiService;
	private readonly DbptContext _dbptContext;

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="apiService"></param>
	/// <param name="dbptContext"></param>
	public UsersController(IApiService apiService, DbptContext dbptContext)
	{
		_apiService = apiService;
		_dbptContext = dbptContext;
	}

	/// <summary>
	/// Para llamar al metodo introducir la url 'https://localhost:5001/api/users/fetch-and-insert'
	/// </summary>
	/// <returns></returns>
	[HttpGet("fetch-and-insert")]
	public async Task<IActionResult> FetchAndInsertUsers()
	{
		string apiUrl = "https://jsonplaceholder.typicode.com/users"; 
		List<Usuario> users = await _apiService.FetchUsersFromApi(apiUrl);

		foreach (var user in users)
		{
			//Identifica usuarios con Id
			if ( _dbptContext.Usuarios.Find(user.Id) == null) 
			{ 
				_dbptContext.Usuarios.Add(user);
			}
		}

		//Añade a la BBDD
		await _dbptContext.SaveChangesAsync();

		return Ok(users); 
	}
}

