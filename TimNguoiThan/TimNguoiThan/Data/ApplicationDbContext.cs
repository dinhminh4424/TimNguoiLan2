using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TimNguoiThan.Models;

namespace TimNguoiThan.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<TimNguoi> TimNguois { get; set; }
        public DbSet<AnhTimNguoi> AnhTimNguois { get; set; }
        public DbSet<BinhLuan> BinhLuans { get; set; }

      

    }
}
