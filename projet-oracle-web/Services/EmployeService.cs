using Oracle.ManagedDataAccess.Client;

namespace projet_oracle_web.Services
{
	public class EmployeeService
	{
		private readonly OracleDbContext _dbContext;

		public EmployeeService(OracleDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Employee>> GetEmployeesAsync()
		{
			var employees = new List<Employee>();
			using (var connection = _dbContext.GetConnection())
			{
				await connection.OpenAsync();
				using (var command = new OracleCommand("SELECT * FROM EMP", connection))
				{
					using (var reader = await command.ExecuteReaderAsync())
					{
						while (await reader.ReadAsync())
						{
							employees.Add(new Employee
							{
								EMPNO = reader.GetInt32(0),
								ENAME = reader.GetString(1),
								JOB = reader.GetString(2),
								MGR = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
								HIREDATE = reader.GetDateTime(4),
								SAL = reader.GetDecimal(5),
								COMM = reader.IsDBNull(6) ? (decimal?)null : reader.GetDecimal(6),
								DEPTNO = reader.GetInt32(7)
							});
						}
					}
				}
			}
			return employees;
		}
    }

}
