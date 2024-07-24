using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Service;

[ApiController]
[Route("api/[controller]")]
public class UsersController : Controller
{
	private readonly IApiService _apiService;
	private readonly DbptContext _context;

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="apiService"></param>
	/// <param name="dbptContext"></param>
	public UsersController(IApiService apiService, DbptContext context)
	{
		_apiService = apiService;
		_context = context;
	}

    public async Task<IActionResult> Index()
    {
        //List<Usuario> users = await _context.Usuarios.ToListAsync();
        var users = await _context.Usuarios
            .Include(x => x.Direccion)
            .Include(x => x.Compania)
            .Include(x => x.Direccion.Geo)
            .ToListAsync();
        return View(users);
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
			if (_context.Usuarios.Find(user.Id) == null) 
			{
                _context.Usuarios.Add(user);
			}

			if (user.Direccion != null)
			{
                _context.Direccions.Add(user.Direccion);
			}

			if (user.Direccion.Geo != null)
			{
                //user.Direccion.Geo.DireccionId = user.Direccion.UserId;
                //user.Direccion.Geo.Direccion = user.Direccion;

                _context.Geos.Add(user.Direccion.Geo);
			}

			if (user.Compania != null)
			{
                //user.Compania.UserId = user.Id; // Establecer la relación
                //user.Compania.User = user;

                _context.Compania.Add(user.Compania);
			}
		}

		//Añade a la BBDD
		await _context.SaveChangesAsync();

		return Ok(users);
    }

    //[HttpPost("api/user/{id}/updateName")]
    //public IActionResult UpdateName(int id, [FromBody] Usuario user)
    //{
    //    if (user == null || id != user.Id)
    //    {
    //        return BadRequest();
    //    }

    //    var existingUser = _context.Usuarios.Find(id);
    //    if (existingUser == null)
    //    {
    //        return NotFound();
    //    }

    //    existingUser.Name = user.Name;
    //    _context.SaveChanges();

    //    return Ok();
    //}

    [HttpPost("{id}/updateName")]
    public IActionResult UpdateName(int id, [FromBody] Usuario user)
    {
        if (user == null || id != user.Id)
        {
            return BadRequest();
        }

        var existingUser = _context.Usuarios.Find(id);
        if (existingUser == null)
        {
            return NotFound();
        }

        existingUser.Name = user.Name;
        _context.SaveChanges();

        return Ok();
    }


}

