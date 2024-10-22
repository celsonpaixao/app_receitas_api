using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_receita.Models
{
    [Table("ratings")]
    public class RatingModel
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("value")]
        [Required]
        public float Value { get; set; }

        [Column("message")]
        public string? Message { get; set; }

        [Column("user_id")]
        public int Id_User { get; set; }
    }
}