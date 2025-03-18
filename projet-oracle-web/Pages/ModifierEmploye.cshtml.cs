using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Oracle.ManagedDataAccess.Client;
using System;
using System.ComponentModel.DataAnnotations;

namespace projet_oracle_web.Pages
{
    public class ModifierEmployeModel : PageModel
    {
        [BindProperty]
        public Services.Employee Employee { get; set; }

        private readonly IConfiguration _configuration;

        // Un seul constructeur avec injection de dépendances
        public ModifierEmployeModel(IConfiguration configuration)
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
                        // Vérifier si le DEPTNO existe dans la table DEPT
                        string checkDeptQuery = "SELECT COUNT(*) FROM DEPT WHERE DEPTNO = :deptno";
                        using (var checkDeptCommand = new OracleCommand(checkDeptQuery, connection))
                        {
                            checkDeptCommand.Parameters.Add(new OracleParameter("deptno", Employee.DEPTNO));
                            int deptExists = Convert.ToInt32(checkDeptCommand.ExecuteScalar());
                            if (deptExists == 0)
                            {
                                TempData["Error"] = "Le département spécifié n'existe pas.";
                                return Page();
                            }
                        }

                        // Mettre à jour l'employé
                        string updateQuery = @"
                        UPDATE EMP 
                        SET ENAME = :ename, JOB = :job, MGR = :MGR, HIREDATE = :hiredate, SAL = :sal, COMM = :COMM, DEPTNO = :deptno
                        WHERE EMPNO = :empno";

                        using (var command = new OracleCommand(updateQuery, connection))
                        {
                            command.Parameters.Add(new OracleParameter("ename", Employee.ENAME));
                            command.Parameters.Add(new OracleParameter("job", Employee.JOB));
                            command.Parameters.Add(new OracleParameter("sal", Employee.MGR));
                            command.Parameters.Add(new OracleParameter("hiredate", Employee.HIREDATE));
                            command.Parameters.Add(new OracleParameter("sal", Employee.SAL));
                            command.Parameters.Add(new OracleParameter("sal", Employee.COMM));
                            command.Parameters.Add(new OracleParameter("deptno", Employee.DEPTNO));
                            command.Parameters.Add(new OracleParameter("empno", Employee.EMPNO));

                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();

                        // Stocker un message de succès avec l'ID de l'employé modifié
                        TempData["Message"] = $"L'employé avec le numéro {Employee.EMPNO} a été modifié avec succès.";
                        TempData["ModifiedEmployee"] = Employee.EMPNO; // ID de la ligne modifiée
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
