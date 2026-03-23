
using LibrosPracticaMvc2JAM.Models;
using LibrosPracticaMvc2JAM.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibrosPracticaMvc2JAM.Controllers
{
    public class ManagedController : Controller
    {
        private RepositoryLibros repo;

        public ManagedController(RepositoryLibros repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            Usuarios usuario = await this.repo.LogInUsuarioAsync(email, password);

            if (usuario != null)
            {
                ClaimsIdentity identity = new ClaimsIdentity(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    ClaimTypes.Name, ClaimTypes.NameIdentifier);

                Claim claimName = new Claim(ClaimTypes.Name, usuario.Nombre);
                Claim claimId = new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString());
                Claim claimEmail = new Claim("Email", usuario.Email);

                identity.AddClaim(claimName);
                identity.AddClaim(claimId);
                identity.AddClaim(claimEmail);

                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal);
                string controller = "Usuarios";
                string action = "Index";
                if (TempData["CONTROLLER"] != null || TempData["ACTION"] != null)
                {
                    controller = TempData["CONTROLLER"].ToString();
                    action = TempData["ACTION"].ToString();
                }


                if (TempData["id"] != null)
                {
                    string id = TempData["id"].ToString();

                    return RedirectToAction(action, controller, new { id = id });
                }
                else
                {
                    return RedirectToAction(action, controller);
                }
            }
            else
            {
                ViewData["MENSAJE"] = "Credenciales incorrectas";
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult ErrorAcceso()
        {
            return View();
        }
    }
}