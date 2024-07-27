using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Service;

//[ApiController]
//[Route("api/[controller]")]
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

    [HttpGet]
    public async Task<IActionResult> Index()
    {
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


                _context.Compania.Add(user.Compania);
			}
		}

		//Añade a la BBDD
		await _context.SaveChangesAsync();

		return Ok(users);
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    { 
        Usuario usuario = await _context.Usuarios.FirstAsync(x => x.Id == id);
        return View(usuario);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(Usuario user)
    {
        _context.Usuarios.Update(user);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

}

