using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api_receita.Models
{
    [Table("tbl_categoria_receita")]
    public class Receita_CategoriaModel
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("id_categoria")]
        public int Id_Categoria { get; set; }

        [Column("id_receita")]
        public int Id_Receita { get; set; }


    }
}