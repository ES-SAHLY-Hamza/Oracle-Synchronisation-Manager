using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using projet_oracle_web.Services;
using System;
using System.Threading.Tasks;

namespace projet_oracle_web.Pages
{
    public class AjouterDepartementModel : PageModel
    {
        [BindProperty]
        public int DEPTNO { get; set; }
        [BindProperty]
        public string DNAME { get; set; }
        [BindProperty]
        public string LOC { get; set; }

        private readonly string _connectionString;

        public AjouterDepartementModel(IConfiguration configuration)
        {
            // R�cup�rer la cha�ne de connexion depuis appsettings.json
            _connectionString = configuration.GetConnectionString("OracleDb");
        }

        public void OnGet()
        {
            // Cette m�thode peut �tre utilis�e pour initialiser la page, si n�cessaire.
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Si le mod�le est invalide, revenir � la page.
            }

            using (OracleConnection conn = new OracleConnection(_connectionString))
            {
                await conn.OpenAsync();

                // Cr�e une commande SQL pour ins�rer un employ�
                string sql = "INSERT INTO DEPT (DEPTNO, DNAME, LOC) VALUES (:DEPTNO, :DNAME, :LOC)";
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    // Param�tres pour s�curiser la requ�te
                    cmd.Parameters.Add(new OracleParameter("DEPTNO", DEPTNO));
                    cmd.Parameters.Add(new OracleParameter("DNAME", DNAME));
                    cmd.Parameters.Add(new OracleParameter("LOC", LOC));
             
                    // D�but de la transaction
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            await cmd.ExecuteNonQueryAsync();
                            transaction.Commit(); // Commit si l'ajout r�ussit
                            return RedirectToPage("/ListerDepartement"); // Redirige vers la liste des dept
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback(); // Annule si erreur
                            ModelState.AddModelError(string.Empty, "Erreur lors de l'ajout de la d�partement : " + ex.Message);
                            return Page();
                        }
                    }
                }
            }
        }
    }
}
