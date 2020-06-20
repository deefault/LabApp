using System;
using System.Data;
using System.Data.SqlClient;
using Npgsql;

namespace LabApp.Server.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }

    public class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            return conn;
        }
    }

    public interface IDbRepository
    {
    }

    public class Repo : IDbRepository
    {
        private readonly IDbConnectionFactory _db;

        public Repo(IDbConnectionFactory dbConnectionFactory)
        {
            _db = dbConnectionFactory ?? throw new ArgumentNullException(nameof(dbConnectionFactory));
        }
    }
}