using Microsoft.AspNetCore.Mvc;
using System.Linq;
using DnDManager.Models;

namespace DnDManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly DnDManagerContext _context;

        public HomeController(DnDManagerContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var result = _context.Character.Where(c => c.Name != null);
            return View(result.ToList());
        }
    }
}