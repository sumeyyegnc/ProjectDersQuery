using System.ComponentModel.DataAnnotations;

namespace ProjectDersQuery.Models
{
    public class Ogrenci
    {
        [Key]
        public int OgrenciId { get; set; }

        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [StringLength(100)]
        public string AdSoyad { get; set; }

        [Required(ErrorMessage = "Email zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        public string Email { get; set; }
    }
}
