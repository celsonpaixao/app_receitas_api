using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api_receita.Models
{
    [Table("favorites")]
    public class FavoritesModel
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("recipe_id")]
        [Required]
        public int Recipe_id { get; set; }

        [Column("user_id")]
        [Required]
        public int User_id { get; set; }
    }
}