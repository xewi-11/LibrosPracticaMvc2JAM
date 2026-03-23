using AyudaExamen.Filters;
using LibrosPracticaMvc2JAM.Helpers;
using LibrosPracticaMvc2JAM.Models;
using LibrosPracticaMvc2JAM.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibrosPracticaMvc2JAM.Controllers
{
    public class PedidosController : Controller
    {
        private RepositoryLibros repo;
        private string PEDIDO_Session_Key = "PedidoId";

        public PedidosController(RepositoryLibros repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index(int usuarioId)
        {


            List<VistaPedidos> pedidoInfo = await this.repo.GetPedidoInfo(usuarioId);
            if (pedidoInfo == null || pedidoInfo.Count == 0)
            {
                return RedirectToAction("Index", "Libro");
            }

            return View(pedidoInfo);
        }



        [HttpGet]
        [AuthorizeUser]
        public async Task<IActionResult> CreatePedido()
        {
            // Recuperar el id de usuario autenticado
            string? userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
            {
                // Si no hay usuario en claims, volvemos al carrito
                return RedirectToAction("GetCarritoSession", "Libro");
            }

            int usuarioId = int.Parse(userIdClaim);

            // Recuperar los cómics del carrito desde sesión
            List<int> comicIds = SessionHelper.GetCarrito(HttpContext.Session);
            if (comicIds == null || comicIds.Count == 0)
            {
                // Si no hay cómics en carrito, redirigimos a la vista del carrito
                return RedirectToAction("GetCarritoSession", "Libro");
            }

            int pedidoId = await this.repo.CreatePedido(usuarioId, comicIds);
            PEDIDO_Session_Key = "${pedidoId}" + pedidoId;
            // Guardar el PedidoId en sesión para poder mostrarlo después
            HttpContext.Session.SetInt32(PEDIDO_Session_Key, pedidoId);

            // Limpiar el carrito una vez creado el pedido
            SessionHelper.LimpiarCarrito(HttpContext.Session);

            return RedirectToAction("Index", "Pedidos", new { usuarioId = usuarioId });
        }

        public IActionResult LimpiarPedido()
        {
            HttpContext.Session.Remove(PEDIDO_Session_Key);
            return RedirectToAction("Index", "Libros");
        }
    }
}
