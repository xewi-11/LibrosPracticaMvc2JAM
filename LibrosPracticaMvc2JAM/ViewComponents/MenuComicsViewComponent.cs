using LibrosPracticaMvc2JAM.Models;
using LibrosPracticaMvc2JAM.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AyudaExamen.ViewComponents
{
    public class MenuComicsViewComponent : ViewComponent
    {
        private RepositoryLibros repo;

        public MenuComicsViewComponent(RepositoryLibros repo)
        {
            this.repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Generos> generos = await this.repo.GetGenerosAsync();
            return View(generos);
        }
    }
}
