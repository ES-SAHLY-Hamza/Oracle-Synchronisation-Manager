using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Oracle.ManagedDataAccess.Client;
using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace projet_oracle_web.Pages
{
    public class SupprimerEmployeModel : PageModel
    {
        [BindProperty]
        public Services.Employee Employee { get; set; }

        private readonly IConfiguration _configuration;

        // Un seul constructeur avec injection de dépendances
        public SupprimerEmployeModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet(int id)
        {
            string connectionString = _configuration.GetConnectionString("OracleDb");
            using (var connection = new OracleConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT EMPNO, ENAME, JOB, MGR, HIREDATE, SAL, COMM, DEPTNO FROM EMP WHERE EMPNO = :id";
                using (var command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(new OracleParameter("id", id));
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Employee = new Services.Employee
                            {
                                EMPNO = reader.GetInt32(0),
                                ENAME = reader.GetString(1),
                                JOB = reader.GetString(2),
                                MGR = !reader.IsDBNull(3) ? reader.GetInt32(3) : (int?)null,
                                HIREDATE = reader.GetDateTime(4),
                                SAL = reader.GetDecimal(5),
                                COMM = !reader.IsDBNull(6) ? reader.GetDecimal(6) : (decimal?)null,
                                DEPTNO = reader.GetInt32(7)
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
                        string deleteQuery = "DELETE FROM EMP WHERE EMPNO = :empno";
                        using (var deleteCommand = new OracleCommand(deleteQuery, connection))
                        {
                            deleteCommand.Parameters.Add(new OracleParameter("empno", Employee.EMPNO));
                            int rowsAffected = deleteCommand.ExecuteNonQuery();

                            Console.WriteLine($"Tentative de suppression pour EMPNO: {Employee.EMPNO}. Lignes affectées: {rowsAffected}");

                            if (rowsAffected > 0)
                            {
                                transaction.Commit();
                                TempData["Message"] = $"L'employé avec le numéro {Employee.EMPNO} a été supprimé avec succès.";
                            }
                            else
                            {
                                transaction.Rollback();
                                TempData["Error"] = "Aucun employé trouvé à supprimer.";
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

            return RedirectToPage("/ListerEmploye");
        }


    }
}
