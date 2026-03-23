using AyudaExamen.Filters;
using LibrosPracticaMvc2JAM.Models;
using LibrosPracticaMvc2JAM.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibrosPracticaMvc2JAM.Controllers
{
    public class UsuariosController : Controller
    {
        private RepositoryLibros repo;
        public UsuariosController(RepositoryLibros repo)
        {
            this.repo = repo;
        }
        [AuthorizeUser]
        public async Task<IActionResult> Index()
        {
            int idUsuario = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "0");

            Usuarios user = await this.repo.FindUsuarioByIdAsync(idUsuario);
            return View(user);
        }
    }
}
