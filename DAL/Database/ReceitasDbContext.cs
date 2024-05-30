using api_receita.Models;
using app_receitas_api.Models;
using Microsoft.EntityFrameworkCore;

namespace api_receita.DAL.Database
{
    public class ReceitasDbContext : DbContext
    {
        public DbSet<UserModel> Tb_User { get; set; }
        public DbSet<RecipeModel> Tb_Receita { get; set; }
        public DbSet<CategoryModel> Tb_Categoria { get; set; }
        public DbSet<AvaluationModel> Tb_Avaliacao { get; set; }
        public DbSet<Recipe_CategoryModel> Tb_Receita_Categoria { get; set; }
        public DbSet<Recipe_AvaluationModel> Tb_Receita_Avaliacao { get; set; }


        public ReceitasDbContext(DbContextOptions<ReceitasDbContext> options)
            : base(options)
        {
        }


    }
}
