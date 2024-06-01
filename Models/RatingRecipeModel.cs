using app_receitas_api.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_receita.Models
{
    [Table("rating_recipe")]
    public class RatingRecipeModel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("rating_id")]
        public int RatingId { get; set; }

        [Required]
        [Column("recipe_id")]
        public int RecipeId { get; set; }
    }
}
