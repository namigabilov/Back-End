using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project_Tenslog.Controllers
{
    [Authorize]
    public class NoficationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
