using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace app_receitas_api.Models
{
    [Table("recipes")]
    public class RecipeModel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("title")]
        [Required]
        public string Title { get; set; }

        [Column("description")]
        [Required]
        public string Description { get; set; }

        [Column("instructions")]
        [Required]
        public string Instructions { get; set; }


        [Column("image_url")]
        public string? ImageURL { get; set; }

        [Column("user_id")]
        [Required]
        public int UserId { get; set; }

        [NotMapped]
        public List<int> Categorias { get; set; } = new List<int>();

        [NotMapped]
        public List<string> Ingredients { get; set; } = new List<string>();

        [NotMapped]
        public List<string> Materials { get; set; } = new List<string>();
    }
}
