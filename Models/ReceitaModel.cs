using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace app_receitas_api.Models
{
    [Table("tbl_receita")]
    public class ReceitaModel
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

        [Column("ingredients")]
        [Required]
        public string Ingredients { get; set; }

        [Column("materials")]
        [Required]
        public string Materials { get; set; }

        [Column("instructions")]
        [Required]
        public string Instructions { get; set; }

        [NotMapped]
        [JsonIgnore]
        [Required]
        public IFormFile ImagePash { get; set; }

        [Column("image_url")]
        public string? ImageURL { get; set; }

        [Column("id_user")]
        [Required]
        public int UserId { get; set; }

        [NotMapped]
        public List<int> Categorias { get; set; } = new List<int>();
    }
}
