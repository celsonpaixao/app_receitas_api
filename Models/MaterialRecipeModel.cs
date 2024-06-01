using app_receitas_api.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_receita.Models
{
    [Table("materials_recipe")]
    public class MaterialRecipeModel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("material_id")]
        public int MaterialId { get; set; }

        [Required]
        [Column("recipe_id")]
        public int RecipeId { get; set; }
    }
}
