using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_receita.Models
{
    [Table("tbl_avaliacao_receita")]
    public class Receita_AvaliacaoModel
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("id_avaliacao")]
        public int Id_Avaliacao { get; set; }

        [Column("id_receita")]
        public int Id_Receita { get; set; }
    }
}
