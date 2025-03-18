using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Oracle.ManagedDataAccess.Client;
using projet_oracle_web.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace projet_oracle_web.Pages
{
    public class SupprimerDepartementModel : PageModel
    {
        [BindProperty]
        public Services.Dept dept { get; set; }

        private readonly IConfiguration _configuration;

        // Un seul constructeur avec injection de d�pendances
        public SupprimerDepartementModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet(int id)
        {
            string connectionString = _configuration.GetConnectionString("OracleDb");
            using (var connection = new OracleConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT DEPTNO, DNAME, LOC FROM DEPT WHERE DEPTNO = :id";
                using (var command = new OracleCommand(query, connection))
                {
                    Console.WriteLine("l'id r�cup�r� est celle de " + id +  "pour la d�partement");
                    command.Parameters.Add(new OracleParameter("id", id));
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            dept = new Services.Dept
                            {
                                DEPTNO = reader.GetInt32(0),
                                DNAME = reader.GetString(1),
                                LOC = reader.GetString(2),
                            };
                        }
                    }
                }
            }
        }
        public IActionResult OnPost()

        {
            if (!ModelState.IsValid)
                return Page();

            string connectionString = _configuration.GetConnectionString("OracleDb");
            using (var connection = new OracleConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string deleteQuery = "DELETE FROM DEPT WHERE DEPTNO = :DEPTNO";
                        using (var deleteCommand = new OracleCommand(deleteQuery, connection))
                        {
                            deleteCommand.Parameters.Add(new OracleParameter("DEPTNO", dept.DEPTNO));
                            int rowsAffected = deleteCommand.ExecuteNonQuery();

                            Console.WriteLine($"Tentative de suppression pour DEPTNO: {dept.DEPTNO}. Lignes affect�es: {rowsAffected}");

                            if (rowsAffected > 0)
                            {
                                transaction.Commit();
                                TempData["Message"] = $"La d�partement avec le num�ro {dept.DEPTNO} a �t� supprim� avec succ�s.";
                            }
                            else
                            {
                                transaction.Rollback();
                                TempData["Error"] = "Aucun d�partement trouv� � supprimer.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        TempData["Error"] = "Erreur lors de la modification : " + ex.Message;
                        return Page();
                    }
                }
            }

            return RedirectToPage("/ListerDepartement");
        }
    }
}
