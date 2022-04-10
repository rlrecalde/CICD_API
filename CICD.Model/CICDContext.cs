using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace CICD.Model
{
    public class CICDContext
    {
        private string _connectionString;
        private DbConnection _dbConnection;
        private DbTransaction _dbTransaction;

        public CICDContext(string connectionString)
        {
            this._connectionString = connectionString;

            if (this._dbConnection == null)
                this._dbConnection = new SqlConnection(this._connectionString);
        }

        public CICDContext(DbConnection dbConnection)
        {
            this._dbConnection = dbConnection;
            this._connectionString = dbConnection.ConnectionString;
        }

        public DbConnection Connection
        {
            get
            {
                if (this._dbConnection.State != ConnectionState.Open)
                {
                    if (string.IsNullOrEmpty(this._dbConnection.ConnectionString))
                        this._dbConnection.ConnectionString = this._connectionString;

                    this._dbConnection.Open();
                }

                return this._dbConnection;
            }
        }

        public void SetTransaction(DbTransaction dbTransaction)
        {
            this._dbTransaction = dbTransaction;
        }

        public DbTransaction GetTransaction()
        {
            return this._dbTransaction;
        }
    }
}