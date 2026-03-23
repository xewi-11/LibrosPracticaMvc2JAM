using LibrosPracticaMvc2JAM.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrosPracticaMvc2JAM.Data
{
    public class LibroContext : DbContext
    {
        public LibroContext(DbContextOptions<LibroContext> options) : base(options)
        {
        }

        public DbSet<Libros> Libros { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Generos> Generos { get; set; }
        public DbSet<VistaPedidos> VistaPedidos { get; set; }
    }
}
