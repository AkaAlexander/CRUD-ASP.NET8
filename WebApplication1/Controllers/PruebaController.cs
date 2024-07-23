using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
	public class PruebaController : Controller
	{
		private readonly DbptContext _context;

		public PruebaController(DbptContext context)
		{
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
	}
}
