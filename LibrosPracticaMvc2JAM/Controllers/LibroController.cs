using LibrosPracticaMvc2JAM.Helpers;
using LibrosPracticaMvc2JAM.Models;
using LibrosPracticaMvc2JAM.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibrosPracticaMvc2JAM.Controllers
{
    public class LibroController : Controller
    {
        private RepositoryLibros repo;
        public LibroController(RepositoryLibros repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            List<Libros> libros = await this.repo.GetLibrosAsync();
            return View(libros);
        }
        public async Task<IActionResult> Detalles(int idLibro)
        {
            Libros libro = await this.repo.FindLibroByIdAsync(idLibro);
            return View(libro);
        }
        public async Task<IActionResult> LibrosGenero(int idGenero)
        {
            List<Libros> libros = await this.repo.GetLibrosByGeneroAsync(idGenero);
            return View(libros);
        }
        public async Task<IActionResult> GetCarritoSession()
        {
            List<int> LibrosId = SessionHelper.GetCarrito(HttpContext.Session);
            if (LibrosId != null)
            {
                List<Libros> libros = new List<Libros>();
                foreach (int id in LibrosId)
                {
                    Libros libro = await this.repo.FindLibroByIdAsync(id);

                    if (libro != null)
                    {
                        libros.Add(libro);

                    }
                }
                ViewData["LIBROSID"] = LibrosId;
                return View(libros);
            }
            else
            {
                return View();
            }

        }

        public async Task<IActionResult> AddToCarritoSesion(int idLibro)
        {

            List<int> LibrosId = SessionHelper.GetCarrito(HttpContext.Session);
            if (LibrosId == null)
            {
                LibrosId = new List<int>();
            }
            if (!LibrosId.Contains(idLibro))
            {
                LibrosId.Add(idLibro);
                SessionHelper.SetCarrito(HttpContext.Session, LibrosId);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> LimpiarCarritoSession()
        {
            SessionHelper.LimpiarCarrito(HttpContext.Session);
            return RedirectToAction("Index");
        }
    }
}
