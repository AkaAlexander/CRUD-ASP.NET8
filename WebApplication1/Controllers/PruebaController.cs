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
			return View(await _context.MiTablas.ToListAsync());
		}
	}
}
