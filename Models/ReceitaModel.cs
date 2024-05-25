using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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

        [Column("image_url")]
        [Required]
        public string ImageURL { get; set; }

        [Column("id_user")]
        [Required]
        public int UserId { get; set; }


    }
}