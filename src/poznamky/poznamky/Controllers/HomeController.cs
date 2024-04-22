using Microsoft.AspNetCore.Mvc;
using Poznamky.Data;

namespace Poznamky.Controllers
{
    public class HomeController : Controller
    {
       
        public IActionResult Index()
        {
            return View();
        }

       
    }
}
