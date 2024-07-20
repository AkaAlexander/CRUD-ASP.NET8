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
	/// Para llamar al metodo introducir la url 'https://localhost:44353/api/users/fetch-and-insert'
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

			if (user.Direccion != null)
			{
				_dbptContext.Direccions.Add(user.Direccion);
			}

			if (user.Direccion.Geo != null)
			{
				//user.Direccion.Geo.DireccionId = user.Direccion.UserId;
				//user.Direccion.Geo.Direccion = user.Direccion;

				_dbptContext.Geos.Add(user.Direccion.Geo);
			}

			if (user.Compania != null)
			{
				//user.Compania.UserId = user.Id; // Establecer la relación
				//user.Compania.User = user;

				_dbptContext.Compania.Add(user.Compania);
			}
		}

		//Añade a la BBDD
		await _dbptContext.SaveChangesAsync();

		return Ok(users); 
	}
}

