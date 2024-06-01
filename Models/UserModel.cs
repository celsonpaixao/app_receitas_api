using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_receita.Models
{
    [Table("users")]
    public class UserModel
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("first_name")]
        public string First_Name { get; set; }

        [Required]
        [Column("last_name")]
        public string Last_Name { get; set; }

        [Required]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [Column("password")]
        public string Password { get; set; }

      
        [Column("image_url")]
        public string? ImageURL { get; set; }
    }
}
