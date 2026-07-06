using System.ComponentModel.DataAnnotations;

namespace ProjectDersQuery.Models
{
    public class Kullanici
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        [StringLength(50)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [StringLength(50)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
