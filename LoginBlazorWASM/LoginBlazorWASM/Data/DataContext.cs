using LoginBlazorWASM.Shared.src;
using Microsoft.EntityFrameworkCore;

namespace LoginBlazorWASM.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base (options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario { Id=1, UserName = "root", Password="root"},
                new Usuario { Id=2, UserName = "root2", Password = "root2"}
                );
        }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}
