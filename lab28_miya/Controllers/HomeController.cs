using lab28_miya.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lab28_miya.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly lab28_miyaContext _context;

        public HomeController(lab28_miyaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var result = _context.CPS.Where(c => c.ID == 1);

            return View(result.ToList());
        }
    }
}
