using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimNguoiThan.Areas.Admin.Models;

namespace TimNguoiThan.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
