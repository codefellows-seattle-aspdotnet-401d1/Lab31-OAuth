using Microsoft.AspNetCore.Mvc;
using IdentityDay2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace IdentityDay2.Controllers
{
    public class HomeController : Controller
    {
        //private field, used for Dependendcy Injection
        private readonly IdentityDay2Context _context;


        //Constructor for Home Controller - a DBContext is required
        public HomeController(IdentityDay2Context context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            // Calling to the CMS table in the database and getting the content for all authorized anouncements
            var result = _context.CMS.Where(c => c.IsAuthorized == true);

            return View(result.ToList());
        }

        [Authorize(Policy = "Medical")]
        public IActionResult ToMedical()
        {
            // Calling to the CMS table in the database and getting the content for all authorized anouncements
            var result = _context.CMS.Where(c => c.IsAuthorized == false && c.Channel == Channel.Medical);

            return View(result.ToList());
        }
    }
}
