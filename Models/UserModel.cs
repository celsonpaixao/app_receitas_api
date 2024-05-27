using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

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
        
        [NotMapped]
        [JsonIgnore]
        public IFormFile? ImagePash { get; set; }
        [Column("image_url")]
        public string? ImageURL { get; set; }
    }
}
