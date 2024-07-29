using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Service;


public class UsersController : Controller
{
    private readonly IApiService _apiService;
    private readonly DbptContext _context;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="apiService"></param>
    /// <param name="context"></param>
    public UsersController(IApiService apiService, DbptContext context)
    {
        _apiService = apiService;
        _context = context;
    }

    /// <summary>
    /// Metodo que devuelve el listado de usuarios
    /// </summary>
    /// <returns></returns>
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
    [HttpGet]
    public async Task<IActionResult> FetchAndInsertUsers()
    {
        string apiUrl = "https://jsonplaceholder.typicode.com/users";
        List<Usuario> users = await _apiService.FetchUsersFromApi(apiUrl);

        foreach (var user in users)
        {
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

                _context.Geos.Add(user.Direccion.Geo);
            }

            if (user.Compania != null)
            {
                _context.Compania.Add(user.Compania);
            }
        }

        await _context.SaveChangesAsync();

        return Ok(users);
    }

    /// <summary>
    /// Metodo carga la vista con el id del usuario
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Vista del usuario</returns>
    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        Usuario usuario = await _context.Usuarios.FirstAsync(x => x.Id == id);
        return View(usuario);
    }

    /// <summary>
    /// Metodo que edita el usuario 
    /// </summary>
    /// <param name="user"></param>
    /// <returns>Vista del listado</returns>
    [HttpPost]
    public async Task<IActionResult> Editar(Usuario user)
    {
        _context.Usuarios.Update(user);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Metodo que Elimina un usuario segun id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Eliminar(int id)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var user = await _context.Usuarios
                    .Include(u => u.Direccion)
                    .ThenInclude(d => d.Geo)
                    .Include(u => u.Compania)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                {
                    return NotFound();
                }

                _context.Usuarios.Remove(user);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                TempData["ErrorMessage"] = "Ocurrió un error al intentar eliminar el usuario.";
                return RedirectToAction(nameof(Index));
            }
        }
    }

}

