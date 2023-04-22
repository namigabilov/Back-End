using Microsoft.AspNetCore.Mvc;

namespace Final_Project_Tenslog.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
