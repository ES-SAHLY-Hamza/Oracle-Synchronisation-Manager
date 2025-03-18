using Oracle.ManagedDataAccess.Client;

namespace projet_oracle_web.Services
{
    public class DepartementService
    {

        private readonly OracleDbContext _dbContext;

        public DepartementService(OracleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Dept>> GetDepartementsAsync()
        {
            var departements = new List<Dept>();
            using (var connection = _dbContext.GetConnection())
            {
                await connection.OpenAsync();
                using (var command = new OracleCommand("SELECT * FROM DEPT", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            departements.Add(new Dept
                            {
                                DEPTNO = reader.GetInt32(0),
                                DNAME = reader.GetString(1),
                                LOC = reader.GetString(2),
                            });
                        }
                    }
                }
            }
            return departements;
        }
    }
}
