using Microsoft.AspNetCore.Mvc;

namespace Car.Controllers
{
    public class CarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
