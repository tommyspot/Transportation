using System;
using System.Configuration;
using System.Data.Common;
using System.Text;
using System.Linq;

namespace Transportation.IntegrationTests
{
    public class DatabaseHelper
    {
        private static ConnectionStringSettings connectionStringSettings;
        private static DbProviderFactory factory;

        public static void ConfigureClarityDB()
        {
            Configure("ClarityDB");
        }

        public static void Configure(string connectionString)
        {
            connectionStringSettings = ConfigurationManager.ConnectionStrings[connectionString];
            factory = DbProviderFactories.GetFactory(connectionStringSettings.ProviderName);
        }

        private static DbConnection CreateConnection()
        {
            DbConnection connection = factory.CreateConnection();
            connection.ConnectionString = connectionStringSettings.ConnectionString;

            return connection;
        }

        private static DbCommand CreateCommand(DbConnection connection, string sql)
        {
            DbCommand command = factory.CreateCommand();
            command.Connection = connection;
            command.CommandText = sql;

            return command;
        }

        private static string GetDatabaseName()
        {
            DbConnectionStringBuilder builder = factory.CreateConnectionStringBuilder();
            builder.ConnectionString = connectionStringSettings.ConnectionString;

            return builder["Initial Catalog"].ToString();
        }

        public static void CreateDatabaseIfNotExists()
        {
            string name = GetDatabaseName();

            string createSql = string.Format("IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = '{0}') CREATE DATABASE {0}", name);

            ExecuteOnMaster(createSql);
        }

        public static void MarkMigrationHistoryTableAsSystem()
        {
            string sql = "sp_ms_marksystemobject __MigrationHistory";
            ExecuteNonQuery(sql);
        }

        public static void DropDatabase()
        {
            string name = GetDatabaseName();
            string sql = string.Format("IF EXISTS(SELECT * FROM sys.databases WHERE name = '{0}') DROP DATABASE {0}", name);

            ExecuteOnMaster(sql);
        }

        private static void ExecuteOnMaster(string sql)
        {
            using (DbConnection connection = factory.CreateConnection())
            {
                connection.ConnectionString = "Server=.\\SQLEXPRESS;Integrated Security=SSPI;";
                connection.Open();

                using (DbCommand command = CreateCommand(connection, sql))
                {
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public static object ExecuteScalar(string sql)
        { 
            object result = null;

            using (DbConnection connection = CreateConnection())
            {
                connection.Open();

                using (DbCommand command = CreateCommand(connection, sql))
                {
                    result = command.ExecuteScalar();
                }
                connection.Close();
            }

            return result;
        }

        public static int ExecuteInteger(string sql)
        {
            return Convert.ToInt32(ExecuteScalar(sql));
        }

        public static bool ExecuteBoolean(string sql)
        {
            return ExecuteInteger(sql) > 0;
        }

        public static void ExecuteNonQuery(string sql)
        {
            using (DbConnection connection = CreateConnection())
            {
                connection.Open();
                using (DbCommand command = CreateCommand(connection, sql))
                {
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public static bool TableExists(string name)
        {
            string sql = string.Format("SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = '{0}'", name);

            return ExecuteBoolean(sql);
        }

        public static bool TableHasRows(string name)
        {
            string sql = string.Format("SELECT COUNT(*) FROM [{0}]", name);

            return ExecuteBoolean(sql);
        }

        public static int TableHasRows(string name, int numberOfRows)
        {
            string sql = string.Format("SELECT COUNT(*) FROM [{0}]", name);

            return ExecuteInteger(sql);
        }

        public static void TruncateTables()
        { 
            StringBuilder sql = new StringBuilder();

            sql.AppendLine("EXEC sp_ms_marksystemobject __MigrationHistory"); // ensure that __MigrationHistory table is not truncated
            sql.AppendLine("EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'");
            sql.AppendLine("EXEC sp_MSForEachTable 'DELETE FROM ?'");
            sql.AppendLine("EXEC sp_MSForEachTable 'ALTER TABLE ? CHECK CONSTRAINT ALL'");

            ExecuteNonQuery(sql.ToString());
        }

        public static void DropTables()
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine("EXEC sp_MSForEachTable 'DROP TABLE ?'");

            ExecuteNonQuery(sql.ToString());
        }

        public static bool ColumnExists(string tableName, string columnName, string dataType)
        {
            var sql = string.Format("SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{0}' AND COLUMN_NAME = '{1}' AND DATA_TYPE = '{2}'", tableName, columnName, dataType);
            return ExecuteBoolean(sql);
        }

        public static bool ColumnExists(string tableName, string columnName)
        {
            var sql = string.Format("SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{0}' AND COLUMN_NAME = '{1}'", tableName, columnName);
            return ExecuteBoolean(sql);
        }

        public static object GetColumnValue(string tableName, string columnName)
        {
            return ExecuteScalar(string.Format("SELECT {0} FROM [{1}]", columnName, tableName));
        }

        public static bool IsIdentityColumn(string tableName, string columnName)
        {
            return ExecuteBoolean(string.Format("SELECT COLUMNPROPERTY(id, name, 'IsIdentity') FROM syscolumns WHERE OBJECT_NAME(id) = '{0}' AND name = '{1}'", tableName, columnName));
        }

        public static void SetUpCaseDB()
        {
            CreateDatabaseIfNotExists();
            DropTables();
        }
    }
}
