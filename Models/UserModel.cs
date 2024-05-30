using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_receita.Models
{
    [Table("tbl_user")]
    public class UserModel
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("primeiro_nome")]
        public string Primeiro_Name { get; set; }

        [Required]
        [Column("ultimo_nome")]
        public string Ultimo_Name { get; set; }

        [Required]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [Column("password")]
        public string Password { get; set; }

        [Required]
        [Column("image_url")]
        public IFormFile? ImageURL { get; set; }
    }
}
