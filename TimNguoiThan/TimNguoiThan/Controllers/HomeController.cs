using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TimNguoiThan.Data;
using TimNguoiThan.Models;

namespace TimNguoiThan.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            this.db = db;
        }

        public IActionResult Index(string? TimKiem)
        {
            ViewBag.TimKiem = TimKiem;
            if (TimKiem.IsNullOrEmpty())
            {
                IEnumerable<TimNguoi> ds = db.TimNguois.Include(u => u.ApplicationUser).Include(u2 => u2.AnhTimNguois).ToList().OrderByDescending(x => x.Id);
                return View(ds);
            }
            else
            {
                IEnumerable<TimNguoi> ds = db.TimNguois.Include(u => u.ApplicationUser).Include(u2 => u2.AnhTimNguois).ToList().OrderByDescending(x => x.Id);
                List<TimNguoi> DSTimkiem = new List<TimNguoi>();
                foreach(TimNguoi i in ds)
                {
                    TimKiem = TimNguoiThan.BoTro.Filter.ChuyenCoDauThanhKhongDau(TimKiem);
                    string r1 = TimNguoiThan.BoTro.Filter.ChuyenCoDauThanhKhongDau(i.HoTen);
                    if (r1.ToUpper().Contains(TimKiem.ToUpper()))
                    {
                        DSTimkiem.Add(i);
                        continue;
                    }
                    string r2 = TimNguoiThan.BoTro.Filter.ChuyenCoDauThanhKhongDau(i.MoTa);
                    if (r2.ToUpper().Contains(TimKiem.ToUpper()))
                    {
                        DSTimkiem.Add(i);
                        continue;
                    }
                    string r3 = TimNguoiThan.BoTro.Filter.ChuyenCoDauThanhKhongDau(i.KhuVuc);
                    if (r3.ToUpper().Contains(TimKiem.ToUpper()))
                    {
                        DSTimkiem.Add(i);
                        continue;
                    }
                    string r4 = TimNguoiThan.BoTro.Filter.ChuyenCoDauThanhKhongDau(i.DaciemNhanDang);
                    if (r4.ToUpper().Contains(TimKiem.ToUpper()))
                    {
                        DSTimkiem.Add(i);
                        continue;
                    }
                }
                return View(DSTimkiem);
            }
           
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
