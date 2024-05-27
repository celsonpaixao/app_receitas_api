using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api_receita.Models
{
    [Table("tbl_categoria")]
    public class CategoriaModel
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("nome")]
        [Required]
        public string Name { get; set; }
    }
}