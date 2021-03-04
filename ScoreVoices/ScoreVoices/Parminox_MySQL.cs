using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;   //System.Data.SqlClient;

namespace Parminox
{
    public class SQL_Class
    {
        private SQLiteConnection conn = null;
        private SQLiteCommand comm = null;
        private SQLiteDataReader Sql_Reader = null;
        private string DBConnectionString = String.Empty;
        //-----------
        public int LastErrorCode { get; private set; } = 0;
        public bool IsConnectionOpened {  get { return conn.State == System.Data.ConnectionState.Open; } }
        public bool TransactionOpened { get; private set; } = false;
        public bool Valid { get; private set; } = false;
        //=================================================
        //  RETURN ERROR CODES
        // 0 - NO ERRORS
        public readonly int setup_database_falure = 159;
        public readonly int open_connection_falure = 114;
        public readonly int begin_transaction_falure = 110;
        public readonly int commit_transaction_falure = 111;
        public readonly int rollback_transaction_falure = 112;
        public readonly int executeNonQuery_falure = 128;
        public readonly int executeReaderDelegate_falure = 91;
        //=================================================
        //  CONSTRUCTOR
        public SQL_Class(string Database_PathName)
        {
            DBConnectionString = $@"Data Source={Database_PathName}; Version=3;";
            try
            {
                if (!File.Exists(Database_PathName))
                {
                    SQLiteConnection.CreateFile(Database_PathName);
                }
                conn = new SQLiteConnection(DBConnectionString, true);
                comm = new SQLiteCommand(String.Empty, conn);
                Valid = true;
                LastErrorCode = 0;
            }
            catch
            {
                DBConnectionString = String.Empty;
                DiscardDatabase();
                LastErrorCode = setup_database_falure;
                Exception ex = new Exception();
                ex.Data.Add("db_exception", LastErrorCode);
                throw new Exception("CREATE_DATABASE_ERROR", ex);
            }
        }
        //=================================================
        public void Dispose()
        {
            if (conn != null && conn.State == System.Data.ConnectionState.Open) conn.Close();
            if (conn != null) conn.Dispose();
            if (comm != null) comm.Dispose();
        }

        private void DiscardDatabase()
        {
            Valid = false;
            Dispose();
        }
        //=================================================
        public void CloseConnection()
        {
            if (!Valid) return;
            if (TransactionOpened) CommitTransaction();
            if (!Valid) return;
            if (conn != null && conn.State == System.Data.ConnectionState.Open) conn.Close();
            LastErrorCode = 0;
        }
        //=================================================
        private bool OpenConnection()
        {
            LastErrorCode = 0;
            if (conn != null && conn.State == System.Data.ConnectionState.Open) return true;
            try
            {
                conn.Open();
                return true;
            }
            catch
            {
                DiscardDatabase();
                LastErrorCode = open_connection_falure;
                Exception ex = new Exception();
                ex.Data.Add("db_exception", LastErrorCode);
                throw new Exception("OPEN_CONNECTION_ERROR", ex);
            }
        }
        //=================================================
        private bool BeginTransaction()
        {
            LastErrorCode = 0;
            if (TransactionOpened) return true;
            if (!OpenConnection()) return false;
            try
            {
                comm.CommandText = @"BEGIN TRANSACTION;";
                comm.ExecuteNonQuery();
                TransactionOpened = true;
                return true;
            }
            catch
            {
                DiscardDatabase();
                LastErrorCode = begin_transaction_falure;
                Exception ex = new Exception();
                ex.Data.Add("db_exception", LastErrorCode);
                throw new Exception("BEGIN_TRANSACTION_ERROR", ex);
            }
        }
        //=================================================
        public bool CommitTransaction()
        {
            LastErrorCode = 0;
            if (!TransactionOpened) return true;
            if (!OpenConnection()) return false;
            try
            {
                comm.CommandText = @"COMMIT TRANSACTION;";
                comm.ExecuteNonQuery();
                TransactionOpened = false;
                return true;
            }
            catch
            {
                DiscardDatabase();
                LastErrorCode = commit_transaction_falure;
                Exception ex = new Exception();
                ex.Data.Add("db_exception", LastErrorCode);
                throw new Exception("COMMIT_TRANSACTION_ERROR", ex);
            }
        }
        //=================================================
        private bool RollBackTransaction()
        {
            LastErrorCode = 0;
            if (!TransactionOpened) return true;
            if (!OpenConnection()) return false;
            try
            {
                comm.CommandText = @"ROLLBACK TRANSACTION;";
                comm.ExecuteNonQuery();
                TransactionOpened = false;
                return true;
            }
            catch
            {
                DiscardDatabase();
                LastErrorCode = rollback_transaction_falure;
                Exception ex = new Exception();
                ex.Data.Add("db_exception", LastErrorCode);
                throw new Exception("ROLLBACK_TRANSACTION_ERROR", ex);
            }
        }
        //=================================================
        //=================================================
        //=================================================
        //   PUBLIC METHODS
        //=================================================
        public bool ExecuteNonQuery(string QueryText, bool Begin_Transaction, bool Commit_Transaction, bool Close_Connection)
        {
            if (!Valid) return false;
            LastErrorCode = 0;
            if (!OpenConnection()) return false;
            if (Begin_Transaction && !BeginTransaction()) return false;
            try
            {
                comm.CommandText = QueryText;
                comm.ExecuteNonQuery();
                if (Commit_Transaction) CommitTransaction();
                if (Close_Connection) CloseConnection();
                return true;
            }
            catch
            {
                DiscardDatabase();
                LastErrorCode = executeNonQuery_falure;
                Exception ex = new Exception();
                ex.Data.Add("db_exception", LastErrorCode);
                throw new Exception("ExecuteNonQuery_ERROR", ex);
            }
        }
        //=================================================
        public bool ExecuteReaderWithCallBackAction(string QueryText, bool Close_Connection, Action<SQLiteDataReader> CallBackAction)
        {
            if (!Valid) return false;
            LastErrorCode = 0;
            if (!OpenConnection()) return false;
            try
            {
                comm.CommandText = QueryText;
                Sql_Reader = comm.ExecuteReader();
                CallBackAction(Sql_Reader);
                Sql_Reader.Close();
                Sql_Reader = null;
                if (Close_Connection) CloseConnection();
                return true;
            }
            catch
            {
                if (Sql_Reader != null) Sql_Reader.Close();
                RollBackTransaction();
                DiscardDatabase();
                LastErrorCode = executeReaderDelegate_falure;
                Exception ex = new Exception();
                ex.Data.Add("db_exception", LastErrorCode);
                throw new Exception("SqlReader_ERROR", ex);
            }           
        }
        //=================================================
        //=================================================
    }
}
