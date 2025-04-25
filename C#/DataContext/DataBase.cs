using Microsoft.Data.Sqlite;
using Ph_Bo_Interfaces;
using Ph_Bo_Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace C_.DataContext
{
    public class DataBase : IDataBase
    { /// <summary>
      ///  Represents a connection to an SQLite database.  
      /// </summary>
        private readonly SqliteConnection _connection;
        public DataBase(string dbname)
        {
            var path = Path.Combine(Environment.CurrentDirectory, dbname);
            _connection = new SqliteConnection(path);
        }
        public bool OpenConnection()
        {
            _connection.Open();
            return true;
        }
        public bool CloseConnection()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
                return true;
            }
            else
            return false;
        }
        public bool AddRow<T>()
        {
            throw new NotImplementedException();
        }



        public bool DeleteRow<T>(T entity)
        {
            throw new NotImplementedException();
        }

        public Contact GetAllRows<T>()
        {
            throw new NotImplementedException();
        }

        public bool GetRow<T>(int id)
        {
            throw new NotImplementedException();
        }

        public bool GetRow<T>(string name)
        {
            throw new NotImplementedException();
        }



        public bool UpdateRow<T>()
        {
            throw new NotImplementedException();
        }
    }
}
