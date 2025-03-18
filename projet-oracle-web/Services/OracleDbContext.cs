using Oracle.ManagedDataAccess.Client;

namespace projet_oracle_web.Services
{
    public class OracleDbContext
    {
        private readonly string _connectionString;

        public OracleDbContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("OracleDb");
        }

        public OracleConnection GetConnection()
        {
            return new OracleConnection(_connectionString);
        }
    }
}
