using System.Data;
using MySql.Data.MySqlClient;

namespace DemoCSharp.model
{
    public static class ConnectionHelper
    {
        private const string ServerName = "localhost";
        private const string DatabaseName = "shb";
        private const string UserName = "root";
        private const string Password = "";
        
        private static MySqlConnection _mySqlConnection;

        public static MySqlConnection GetConnection()
        {
            if (_mySqlConnection != null && _mySqlConnection.State != ConnectionState.Closed) return _mySqlConnection;
            _mySqlConnection = new MySqlConnection($"Server={ServerName};Database={DatabaseName};Uid={UserName};Pwd={Password};SslMode=none");
            _mySqlConnection.Open();
            return _mySqlConnection;
        }
        

        public static void CloseConnection()
        {
            if (_mySqlConnection == null || _mySqlConnection.State == ConnectionState.Closed)
            {
                return;
            }
            _mySqlConnection.Close();
        }
        

    }
}