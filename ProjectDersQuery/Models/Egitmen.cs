using System.ComponentModel.DataAnnotations;

namespace ProjectDersQuery.Models
{
    public class Egitmen
    {
        [Key]
        public int EgitmenId { get; set; }

        [Required(ErrorMessage = "Eğitmen adı zorunludur.")]
        [StringLength(100)]
        public string EgitmenAdi { get; set; }

        [Required(ErrorMessage = "Branş zorunludur.")]
        [StringLength(100)]
        public string Brans { get; set; }
    }
}
