using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TimNguoiThan.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() 
        {
            this.TimNguois = new HashSet<TimNguoi>();
            this.BinhLuans = new HashSet<BinhLuan>();
        }

        [Required]
        public string FullName { get; set; }
        public string? Address { get; set; }
        
        public DateTime? NgaySinh { get; set; }
        public string? CCCD { get; set; }

        public virtual ICollection<TimNguoi>? TimNguois { get; set; }
        public virtual ICollection<BinhLuan>? BinhLuans { get; set; }
    }
}
