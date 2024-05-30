using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api_receita.Models
{
    [Table("tbl_avaliacao")]
    public class AvaluationModel
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("value")]
        [Required]
        public float Value { get; set; }

        [Column("message")]
        public string? Message { get; set; }

        [Column("id_user")]
        public int Id_User { get; set; }
    }
}