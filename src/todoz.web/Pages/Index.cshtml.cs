using Microsoft.AspNetCore.Mvc.RazorPages;

namespace todoz.web.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet() =>
            // Passa a URL da API para o ViewData
            ViewData["ApiUrl"] = Environment.GetEnvironmentVariable("API_URL") ?? "http://localhost:5021";
    }
}