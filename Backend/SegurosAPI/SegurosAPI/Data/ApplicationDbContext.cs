using Microsoft.EntityFrameworkCore;
using SegurosAPI.Modelo;

namespace SegurosAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Seguro> Seguro { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<ClienteSeguro> ClienteSeguro { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*Combinacion de id como llave primaria de la tabla*/
            modelBuilder.Entity<ClienteSeguro>().HasKey(ci => new { ci.clienteid, ci.seguroid });
        }

    }
}
