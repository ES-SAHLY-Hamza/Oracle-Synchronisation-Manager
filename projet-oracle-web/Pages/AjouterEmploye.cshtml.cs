using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using projet_oracle_web.Services;
using System;
using System.Threading.Tasks;

namespace projet_oracle_web.Pages
{
    public class AjouterEmployeModel : PageModel
    {
        [BindProperty]
        public int EmpNo { get; set; }
        [BindProperty]
        public string Ename { get; set; }
        [BindProperty]
        public string Job { get; set; }
        [BindProperty]
        public decimal MGR { get; set; }
        [BindProperty]
        public DateTime HireDate { get; set; }
        [BindProperty]
        public decimal Sal { get; set; }
        [BindProperty]
        public decimal Comm { get; set; }
        [BindProperty]
        public decimal DEPTNO { get; set; }


        private readonly string _connectionString;

        public AjouterEmployeModel(IConfiguration configuration)
        {
            // Récupérer la chaîne de connexion depuis appsettings.json
            _connectionString = configuration.GetConnectionString("OracleDb");
        }

        public void OnGet()
        {
            // Cette méthode peut être utilisée pour initialiser la page, si nécessaire.
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Si le modèle est invalide, revenir à la page.
            }

            using (OracleConnection conn = new OracleConnection(_connectionString))
            {
                await conn.OpenAsync();

                // Crée une commande SQL pour insérer un employé
                string sql = "INSERT INTO EMP (EMPNO, ENAME, JOB, MGR, HIREDATE, SAL, COMM, DEPTNO) VALUES (:EmpNo, :Ename, :Job, :MGR, :HireDate, :Sal, :COMM, :DEPTNO)";
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    // Paramètres pour sécuriser la requête
                    cmd.Parameters.Add(new OracleParameter("EmpNo", EmpNo));
                    cmd.Parameters.Add(new OracleParameter("Ename", Ename));
                    cmd.Parameters.Add(new OracleParameter("Job", Job));
                    cmd.Parameters.Add(new OracleParameter("MGR", MGR));
                    cmd.Parameters.Add(new OracleParameter("HireDate", HireDate));
                    cmd.Parameters.Add(new OracleParameter("Sal", Sal));
                    cmd.Parameters.Add(new OracleParameter("COMM", Comm));
                    cmd.Parameters.Add(new OracleParameter("DEPTNO", DEPTNO));

                    // Début de la transaction
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            await cmd.ExecuteNonQueryAsync();
                            transaction.Commit(); // Commit si l'ajout réussit
                            return RedirectToPage("/ListerEmploye"); // Redirige vers la liste des employés
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback(); // Annule si erreur
                            ModelState.AddModelError(string.Empty, "Erreur lors de l'ajout de l'employé : " + ex.Message);
                            return Page();
                        }
                    }
                }
            }
        }
    }
}
