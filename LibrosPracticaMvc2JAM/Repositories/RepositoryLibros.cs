using LibrosPracticaMvc2JAM.Data;
using LibrosPracticaMvc2JAM.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrosPracticaMvc2JAM.Repositories
{
    public class RepositoryLibros
    {
        private LibroContext context;

        public RepositoryLibros(LibroContext context)
        {
            this.context = context;
        }

        public async Task<Usuarios> FindUsuarioByIdAsync(int idUsuario)
        {
            return await this.context.Usuarios.Where(x => x.IdUsuario == idUsuario).FirstOrDefaultAsync();
        }

        public async Task<List<Generos>> GetGenerosAsync()
        {
            return await this.context.Generos.ToListAsync();
        }
        public async Task<List<Libros>> GetLibrosAsync()
        {
            return await this.context.Libros.ToListAsync();
        }
        public async Task<List<Libros>> GetLibrosByGeneroAsync(int idGenero)
        {
            return await this.context.Libros.Where(x => x.IdGenero == idGenero).ToListAsync();
        }

        public async Task<Libros> FindLibroByIdAsync(int idLibro)
        {
            return await this.context.Libros.Where(x => x.IdLibro == idLibro).FirstOrDefaultAsync();
        }
        public async Task<Usuarios> LogInUsuarioAsync(string email, string pass)
        {
            Usuarios usuario = await this.context.Usuarios.FirstOrDefaultAsync(z => z.Email == email && z.Pass == pass);

            return usuario;
        }
        public async Task<List<VistaPedidos>> GetPedidoInfo(int id)
        {
            var pedidos = await this.context.VistaPedidos
                .Where(p => p.IdUsuario == id)
                .ToListAsync();

            return pedidos;
        }

        public async Task<int> CreatePedido(int usuarioId, List<int> librosIds)
        {
            // Obtener el máximo PedidoId de la BD, si la tabla está vacía usar 0 (para sumarle 1)
            int maxPedidoId = await this.context.Pedidos
                .DefaultIfEmpty()
                .MaxAsync(p => (int?)p.IdPedido) ?? 0;



            int maxFactura = await this.context.Pedidos
                .DefaultIfEmpty()
                .MaxAsync(p => (int?)p.IdFactura) ?? 0;

            int nuevoPedidoId = maxPedidoId + 1;

            // Crear un registro Pedido por cada comic
            foreach (int libro in librosIds)
            {
                maxPedidoId++;

                maxFactura++;
                Pedido pedido = new Pedido
                {
                    IdPedido = maxPedidoId,
                    IdUsuario = usuarioId,
                    IdLibro = libro,
                    IdFactura = maxFactura,
                    Fecha = DateTime.Now,
                    Cantidad = 1
                };
                this.context.Pedidos.Add(pedido);
                await this.context.SaveChangesAsync();
            }



            return nuevoPedidoId; // Retorna el Id principal que identifica la factura base del grupo
        }
    }
}
