using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace projet_oracle_web.Pages
{
    public class AuthentificationModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public IActionResult OnPost()
        {
            // Vérification des identifiants
            if (Username == "SCOTT" && Password == "TIGER")
            {
                return RedirectToPage("/ChoiciData");
            }

            // Message d'erreur en cas d'échec
            ErrorMessage = "Identifiant ou mot de passe incorrect.";
            return Page();
        }
    }
}
