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
		string apiUrl = "https://jsonplaceholder.typicode.com/users"; // Reemplaza esto con la URL real de tu API
		List<Usuario> users = await _apiService.FetchUsersFromApi(apiUrl);

		// Aquí puedes llamar a otro servicio para insertar los usuarios en la base de datos
		foreach (var user in users)
		{
			//if (_dbptContext.Usuarios.Find(user.Id) == null) // Evita duplicados
			//{
				Usuario usuario = new Usuario() 
				{ 
					Id = user.Id,
					Name = user.Name,
					Username = user.Name,
					Email = user.Email,
					Phone = user.Phone,
					Website = user.Website
				};

				_dbptContext.Usuarios.Add(usuario);

				if (user.Direccion != null)
				{
					user.Direccion.UserId = user.Id; 
					user.Direccion.User = user;
				}

				if (user.Compania != null)
				{
					user.Compania.UserId = user.Id; 
					user.Compania.User = user;
				}


				if (user.Direccion != null)
				{
					_dbptContext.Direccions.Add(user.Direccion);
				}

				if (user.Compania != null)
				{
					_dbptContext.Compania.Add(user.Compania);
				}
			//}
		}

		await _dbptContext.SaveChangesAsync();

		return Ok(users); // O cualquier otra acción que desees realizar con los datos
	}
}

