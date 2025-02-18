using LoginBlazorWASM.Shared.src;
using Microsoft.EntityFrameworkCore;

namespace LoginBlazorWASM.Data
{
    public partial class DataContext : DbContext
    {
        public DataContext()
        {

        }
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("usuarios_pkey");

                entity.ToTable("usuarios");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UserName)
                    .HasMaxLength(200)
                    .HasColumnName("userName");
                entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
                entity.Property(e => e.PasswordSalt).HasColumnName("password_salt");
                entity.Property(e => e.Rol)
                    .HasMaxLength(200)
                    .HasColumnName("rol");
                entity.Property(e => e.Token)
                    .HasColumnType("character varying")
                    .HasColumnName("token");
            });
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}