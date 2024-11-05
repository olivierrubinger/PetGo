using Microsoft.AspNetCore.Mvc;

namespace todoz.api.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Error()
        {
            // Você pode adicionar lógica para registrar o erro ou exibir uma mensagem personalizada
            return View();
        }
    }
}