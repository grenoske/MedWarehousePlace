using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
