using Microsoft.AspNetCore.Mvc;

namespace Fitally.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
