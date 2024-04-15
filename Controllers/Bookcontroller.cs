using Microsoft.AspNetCore.Mvc;

namespace Library_API_1.Controllers
{
    public class Bookcontroller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
