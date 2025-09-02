using Microsoft.EntityFrameworkCore;
using DataVault.Models;

namespace DataVault.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuario { get; set; }     
        public DbSet<Feedback> Feedbacks { get; set; }  // Tabela Feedbacks
        public DbSet<Perfil> Perfis { get; set; }
        public DbSet<Files> Files { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }
    }
}