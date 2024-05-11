using Microsoft.AspNetCore.Mvc;
using PL.Models;

namespace PL.Controllers
{
    public class ItemController : Controller
    {
        public IActionResult Index(int page = 1)
        {
            return View();
        }
    }
}
