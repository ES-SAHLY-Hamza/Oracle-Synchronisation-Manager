using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Oracle.ManagedDataAccess.Client;
using projet_oracle_web.Services;
using System;
using System.ComponentModel.DataAnnotations;

namespace projet_oracle_web.Pages
{
    public class ModifierDepartementModel : PageModel
    {
        [BindProperty]
        public Services.Dept dept { get; set; }

        private readonly IConfiguration _configuration;

        // Un seul constructeur avec injection de dépendances
        public ModifierDepartementModel(IConfiguration configuration)
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
                    Console.WriteLine(id);
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
                      // Mettre à jour l'employé
                        string updateQuery = @"
                        UPDATE DEPT 
                        SET DNAME = :DNAME, LOC = :LOC WHERE DEPTNO = :DEPTNO";
                        using (var command = new OracleCommand(updateQuery, connection))
                        {
                            command.Parameters.Add(new OracleParameter("DNAME", dept.DNAME));
                            command.Parameters.Add(new OracleParameter("LOC", dept.LOC));
                            command.Parameters.Add(new OracleParameter("DEPTNO", dept.DEPTNO));

                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();

                        // Stocker un message de succès avec l'ID de dept modifié
                        TempData["Message"] = $"La département avec le numéro {dept.DEPTNO} a été modifié avec succès.";
                        TempData["ModifiedDept"] = dept.DEPTNO; // ID de la ligne modifiée
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
