using Microsoft.AspNetCore.Mvc;

namespace Final_Project_Tenslog.Controllers
{
    public class DirectController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MobileChat()
        {
            return View();
        }
    }
}
