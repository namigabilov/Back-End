using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Final_Project_Tenslog.Controllers
{
    [Authorize]
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
