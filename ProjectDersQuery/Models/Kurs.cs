using System.ComponentModel.DataAnnotations;

namespace ProjectDersQuery.Models
{
    public class Kurs
    {
        [Key]
        public int KursId { get; set; }

        [Required(ErrorMessage = "Kurs adı zorunludur.")]
        [StringLength(150)]
        public string KursAdi { get; set; }

        [Required(ErrorMessage = "Ücret zorunludur.")]
        [Range(0, int.MaxValue, ErrorMessage = "Ücret negatif olamaz.")]
        public int Ucret { get; set; }
    }
}
