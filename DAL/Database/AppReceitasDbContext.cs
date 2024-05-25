using api_receita.Models;
using Microsoft.EntityFrameworkCore;

namespace api_receita.DAL.Database
{
    public class AppReceitasDbContext : DbContext
    {
        public DbSet<UserModel> Tb_User { get; set; }

        public AppReceitasDbContext(DbContextOptions<AppReceitasDbContext> options)
            : base(options)
        {
        }

       
    }
}
