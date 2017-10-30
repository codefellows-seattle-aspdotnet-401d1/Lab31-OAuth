using Microsoft.AspNetCore.Mvc;

namespace lab31_brian.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
