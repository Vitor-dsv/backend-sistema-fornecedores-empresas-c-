using Microsoft.EntityFrameworkCore;

namespace BackEnd.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Fornecedor>()
                .HasOne(f => f.Empresa)
                .WithMany(e => e.Fornecedores)
                .HasForeignKey(f => f.EmpresaFK)
                .IsRequired();
        }
              
        public DbSet<Fornecedor> Fornecedor { get; set; }
        public DbSet<Empresa> Empresa { get; set; }
    }
}
