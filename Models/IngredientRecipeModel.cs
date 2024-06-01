using app_receitas_api.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_receita.Models
{
    [Table("ingredients_recipe")]
    public class IngredientRecipeModel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("ingredient_id")]
        public int IngredientId { get; set; }

        [Required]
        [Column("recipe_id")]
        public int RecipeId { get; set; }
    }
}
