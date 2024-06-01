using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api_receita.Models
{
    [Table("category_recipe")]
    public class Recipe_CategoryModel
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("category_id")]
        public int CategoryId { get; set; }

        [Column("recipe_id")]
        public int RecipeId { get; set; }


    }
}