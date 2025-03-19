using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimNguoiThan.Models
{
    [Table("BinhLuan")]
    public class BinhLuan
    {

        [Key]
        public int Id { get; set; }

        public string NoiDung  { get; set; }
        public string? HinhAnh { get; set; }
        public DateTime NgayBinhLuan { get; set; } = DateTime.Now;
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser? ApplicationUser { get; set; }

        public int? IdBaiViet { get; set; }
        [ForeignKey("IdBaiViet")]
        public virtual TimNguoi? TimNguoi { get; set; }

    }
}
