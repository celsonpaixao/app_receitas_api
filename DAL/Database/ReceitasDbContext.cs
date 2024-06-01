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
        public DbSet<RatingModel> Tb_Avaliacao { get; set; }
        public DbSet<Recipe_CategoryModel> Tb_RecipeCategory { get; set; }
        public DbSet<RatingRecipeModel> Tb_Receita_Avaliacao { get; set; }
        public DbSet<MaterialRecipeModel> Tb_Materials_Recipe { get; set; }
        public DbSet<IngredientRecipeModel> Tb_Ingredients_Recipe { get; set; }
        public DbSet<MaterialsModel> Tb_Materials { get; set; }
        public DbSet<IngredientsModel> Tb_Ingredients { get; set; }
        public DbSet<FavoritesModel> Tb_Favorite { get; set; }


        public ReceitasDbContext(DbContextOptions<ReceitasDbContext> options)
            : base(options)
        {
        }


    }
}
