using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using projet_oracle_web.Services;

namespace projet_oracle_web.Pages
{
    public class ListerDepartementModel : PageModel
    {
        private readonly DepartementService _departementService;

        public ListerDepartementModel(DepartementService departementService)
        {
            _departementService = departementService;
        }

        public List<Dept> departements { get; set; }

        public async Task OnGetAsync()
        {
            departements = await _departementService.GetDepartementsAsync();
        }
    }
}
