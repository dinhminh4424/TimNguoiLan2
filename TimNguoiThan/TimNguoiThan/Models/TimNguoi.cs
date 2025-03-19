using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimNguoiThan.Models
{
    [Table("TimNguoi")]
    public class TimNguoi
    {
        
        public  TimNguoi()
        {
            this.AnhTimNguois = new HashSet<AnhTimNguoi>();
            this.BinhLuans = new HashSet<BinhLuan>();
        }

        [Key]
        public int Id { get; set; }
        public string? HoTen { get; set; }
        public string? TieuDe { get; set; }
        [DataType(DataType.Html)]
        public string? MoTa { get; set; }
        public string DaciemNhanDang { get; set; } 
        public int? GioiTinh { get; set; }
        public string? TrangThai { get; set; } = "Đang Tìm Kiếm";
        public string? KhuVuc { get; set; }
        public DateTime NgayDang { get; set; } = DateTime.Now;
        public  ICollection<AnhTimNguoi>? AnhTimNguois { get; set; }

        public string? IdNguoiDung { get; set; }
        [ForeignKey("IdNguoiDung")]
        public  ApplicationUser? ApplicationUser { get; set; }

        public  ICollection<BinhLuan>? BinhLuans { get; set; }
    }
}
