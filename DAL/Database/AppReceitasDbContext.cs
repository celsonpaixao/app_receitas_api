using api_receita.Models;
using app_receitas_api.Models;
using Microsoft.EntityFrameworkCore;

namespace api_receita.DAL.Database
{
    public class AppReceitasDbContext : DbContext
    {
        public DbSet<UserModel> Tb_User { get; set; }
        public DbSet<ReceitaModel> Tb_Receita { get; set; }
        public DbSet<CategoriaModel> Tb_Categoria { get; set; }
        public DbSet<AvaliacaoModel> Tb_Avaliacao { get; set; }
        public DbSet<Receita_CategoriaModel> Tb_Receita_Categoria { get; set; }
        public DbSet<Receita_AvaliacaoModel> Tb_Receita_Avaliacao { get; set; }


        public AppReceitasDbContext(DbContextOptions<AppReceitasDbContext> options)
            : base(options)
        {
        }


    }
}
