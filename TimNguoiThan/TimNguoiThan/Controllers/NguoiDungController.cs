using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimNguoiThan.Data;
using TimNguoiThan.Models;

namespace TimNguoiThan.Controllers
{
    public class NguoiDungController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private ApplicationDbContext db;
        public NguoiDungController(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            this.userManager = userManager;
            this.db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ThongTinCaNhan()
        {
            ApplicationUser x = await userManager.GetUserAsync(User);
            List<TimNguoi> timNguois = await db.TimNguois.Include(u => u.AnhTimNguois).Where(i => i.IdNguoiDung == x.Id).ToListAsync();
            ViewBag.CacBaiDang = timNguois;
            return View(x);

        }
        public async Task<IActionResult> EditTaiKhoan()
        {
            ApplicationUser x = await userManager.GetUserAsync(User);
            return View(x);
        }


        
    }
}
