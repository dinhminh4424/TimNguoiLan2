using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using TimNguoiThan.Data;
using TimNguoiThan.Models;

namespace TimNguoiThan.Controllers
{
    public class TimNguoiController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<ApplicationUser> userManager;
        private static readonly IEnumerable<string> TinhThanhIEnumerable = new List<string>
    {
        "Hà Nội",
        "Hồ Chí Minh",
        "Đà Nẵng",
        "Hải Phòng",
        "Cần Thơ",
        "An Giang",
        "Bà Rịa - Vũng Tàu",
        "Bắc Giang",
        "Bắc Kạn",
        "Bạc Liêu",
        "Bắc Ninh",
        "Bến Tre",
        "Bình Định",
        "Bình Dương",
        "Bình Phước",
        "Bình Thuận",
        "Cà Mau",
        "Cao Bằng",
        "Đắk Lắk",
        "Đắk Nông",
        "Điện Biên",
        "Đồng Nai",
        "Đồng Tháp",
        "Gia Lai",
        "Hà Giang",
        "Hà Nam",
        "Hà Tĩnh",
        "Hải Dương",
        "Hậu Giang",
        "Hòa Bình",
        "Hưng Yên",
        "Khánh Hòa",
        "Kiên Giang",
        "Kon Tum",
        "Lai Châu",
        "Lâm Đồng",
        "Lạng Sơn",
        "Lào Cai",
        "Long An",
        "Nam Định",
        "Nghệ An",
        "Ninh Bình",
        "Ninh Thuận",
        "Phú Thọ",
        "Quảng Bình",
        "Quảng Nam",
        "Quảng Ngãi",
        "Quảng Ninh",
        "Quảng Trị",
        "Sóc Trăng",
        "Sơn La",
        "Tây Ninh",
        "Thái Bình",
        "Thái Nguyên",
        "Thanh Hóa",
        "Thừa Thiên Huế",
        "Tiền Giang",
        "Trà Vinh",
        "Tuyên Quang",
        "Vĩnh Long",
        "Vĩnh Phúc",
        "Yên Bái",
        "Phú Yên"
    };

        public  TimNguoiController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }



        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ThemNguoiCanTim()
        {
            ViewBag.DanhSachTinhThanh = TinhThanhIEnumerable;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThemNguoiCanTim(TimNguoi timNguoi, List<IFormFile> DSHinhAnhCapNhat)
        {
            if (ModelState.IsValid)
            {
                if(DSHinhAnhCapNhat == null)
                {
                    ModelState.AddModelError("Lỗi", "Chưa Có Hình Ảnh");
                    ViewBag.DanhSachTinhThanh = TinhThanhIEnumerable;
                    return View(timNguoi);
                }
                db.Add(timNguoi);
                await db.SaveChangesAsync();
                int d = 0;
                foreach(IFormFile i in DSHinhAnhCapNhat)
                {
                    AnhTimNguoi x = new AnhTimNguoi();
                    x.IdNguoiCanTim = timNguoi.Id;
                    if(d==0)
                    {
                        x.TrangThai = 1;
                        d++;
                    }
                    else
                    {
                        x.TrangThai = 0;
                    }
                    x.HinhAnh = await SaveImage(i, "AnhNguoiCanTim");
                    db.AnhTimNguois.Add(x);
                    await db.SaveChangesAsync();
                }
                return RedirectToAction("ThongTinCaNhan", "NguoiDung");
            }

            ViewBag.DanhSachTinhThanh = TinhThanhIEnumerable;
            return View(timNguoi);
        }

        public async Task<string> SaveImage(IFormFile ImageURL, string subFolder)
        {
            if (ImageURL == null || ImageURL.Length == 0)
            {
                throw new ArgumentException("File không hợp lệ!");
            }

            // Đường dẫn thư mục lưu ảnh trong wwwroot/uploads/tin-tuc
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", subFolder);

            // Tạo thư mục nếu chưa tồn tại
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Tạo tên file duy nhất để tránh trùng lặp
            string fileExtension = Path.GetExtension(ImageURL.FileName);
            string fileName = Path.GetFileNameWithoutExtension(ImageURL.FileName);
            string uniqueFileName = fileName + "_" + Guid.NewGuid().ToString("N") + fileExtension;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Lưu file vào thư mục
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await ImageURL.CopyToAsync(fileStream);
            }

            // Trả về đường dẫn tương đối để hiển thị ảnh trên web
            return $"/uploads/{subFolder}/{uniqueFileName}";
        }


        public void DeleteImage(string ImageURL, string subFolder)
        {
            if (string.IsNullOrEmpty(ImageURL))
            {
                throw new ArgumentException("Đường dẫn ảnh không hợp lệ!");
            }

            // Lấy đường dẫn tuyệt đối của ảnh trong thư mục wwwroot/uploads/
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", subFolder);
            string filePath = Path.Combine(uploadsFolder, Path.GetFileName(ImageURL));

            // Kiểm tra nếu file tồn tại thì xóa
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }


        public async Task<IActionResult> ChiTietBaiTimNguoi(int id)
        {
            ApplicationUser x = await userManager.GetUserAsync(User);
            if(x != null)
            {
                
                TimNguoi y = db.TimNguois.Include(u => u.AnhTimNguois).FirstOrDefault(i => i.Id == id);
                ApplicationUser us = await userManager.FindByIdAsync(y.IdNguoiDung);
                ViewBag.NguoiTim = us;
                ViewBag.DSHinhAnh = await db.AnhTimNguois.Where(i => i.IdNguoiCanTim == y.Id).ToListAsync();
                List<BinhLuan> DSBinhLuan = db.BinhLuans.Include(u => u.ApplicationUser).Where(i => i.IdBaiViet ==  id).OrderByDescending(z => z.NgayBinhLuan).ToList();
                ViewBag.DSBinhLuan = DSBinhLuan;
                return View(y);
            }
            return RedirectToAction("ThongTinCaNhan", "NguoiDung");
        }

        [HttpPost]
        public async Task<IActionResult> ThemBinhLuan(int IdBaiViet,string UserId,string NoiDung, IFormFile? HinhAnh)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Lấy ID của người dùng hiện tại

                // Xử lý upload hình ảnh
               if(ModelState.IsValid)
               {
                    BinhLuan x = new BinhLuan();
                    if(HinhAnh != null)
                    {
                        x.HinhAnh = await SaveImage(HinhAnh, "BinhLuan");
                    }
                    x.NoiDung = NoiDung;
                    x.UserId = userId;
                    x.IdBaiViet = IdBaiViet;

                    db.BinhLuans.Add(x);
                    await db.SaveChangesAsync();
                }
                return RedirectToAction("ChiTietBaiTimNguoi", new { id = IdBaiViet }); // Quay lại trang chi tiết

               
            }
            ModelState.AddModelError("Lỗi", "Chưua Đăng Nhập");
            return RedirectToAction("ChiTietBaiTimNguoi", new { id = IdBaiViet }); // Quay lại trang chi tiết
        }

        public async Task<IActionResult> CapNhatBaiViet(int id)
        {
            if(User.Identity.IsAuthenticated)
            {
                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                TimNguoi x = db.TimNguois
                                    .Include(u => u.ApplicationUser)
                                    .Include(u => u.AnhTimNguois)
                                    .Include(u => u.BinhLuans)
                                    .FirstOrDefault(i => i.Id ==  id);
                if(x.IdNguoiDung == userid)
                {
                    ViewBag.DanhSachTinhThanh = TinhThanhIEnumerable;
                    ViewBag.DanhSachHinhAnh = db.AnhTimNguois.Where(i => i.IdNguoiCanTim == id).ToList();
                    return View(x);
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            ViewBag.DanhSachTinhThanh = TinhThanhIEnumerable;
            ViewBag.DanhSachHinhAnh = db.AnhTimNguois.Where(i => i.IdNguoiCanTim == id).ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CapNhatBaiViet(TimNguoi x, List<IFormFile>? DSHinhAnhCapNhat)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                TimNguoi y = db.TimNguois
                                    .Include(u => u.ApplicationUser)
                                    .Include(u => u.AnhTimNguois)
                                    .Include(u => u.BinhLuans)
                                    .FirstOrDefault(i => i.Id == x.Id);
                if (x.IdNguoiDung == userid)
                {
                    if(ModelState.IsValid)
                    {
                        y.MoTa = x.MoTa;
                        y.TieuDe = x.TieuDe;
                        y.DaciemNhanDang = x.DaciemNhanDang;
                        y.GioiTinh = x.GioiTinh;
                        y.TrangThai = x.TrangThai;

                        if(DSHinhAnhCapNhat != null)
                        {
                            List<AnhTimNguoi> dsHinhAnh = db.AnhTimNguois.Where( i => i.IdNguoiCanTim == x.Id).ToList();
                            
                            if(dsHinhAnh != null)
                            {
                                foreach (AnhTimNguoi i in dsHinhAnh)
                                {
                                    db.AnhTimNguois.Remove(i);
                                    db.SaveChanges();
                                }
                            }
                            int d = 0;
                            foreach (IFormFile i in DSHinhAnhCapNhat)
                            {
                                AnhTimNguoi z = new AnhTimNguoi();
                                z.IdNguoiCanTim = y.Id;
                                if (d == 0)
                                {
                                    z.TrangThai = 1;
                                    d++;
                                }
                                else
                                {
                                    z.TrangThai = 0;
                                }
                                z.HinhAnh = await SaveImage(i, "AnhNguoiCanTim");
                                db.AnhTimNguois.Add(z);
                                await db.SaveChangesAsync();
                            }
                            return RedirectToAction("ChiTietBaiTimNguoi",new {id = x.Id});

                        }
                    }
                    ViewBag.DanhSachHinhAnh = db.AnhTimNguois.Where(i => i.IdNguoiCanTim == x.Id).ToList();
                    return View(x);
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            ViewBag.DanhSachHinhAnh = db.AnhTimNguois.Where(i => i.IdNguoiCanTim == x.Id).ToList();
            return View(x);
        }
    }
}
