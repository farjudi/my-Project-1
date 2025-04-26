using Microsoft.Data.Sqlite;
using Ph_Bo_Interfaces;
using Ph_Bo_Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
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
        public DataBase(string dbName)
        {
            var path = Path.Combine(Environment.CurrentDirectory, dbName);
            _connection = new SqliteConnection($"Data Source={path}");


        }
        public bool OpenConnection()
        {
            try
            {
                _connection.Open();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening connection: {ex.Message}");
                return false;
            }
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

        public bool CreateTableContact()
        {
            string createTableQuery = @"
             CREATE TABLE IF NOT EXISTS contacts (    
             Id INTEGER   PRIMARY KEY AUTOINCREMENT,
             First_name   Text NOT NULL,       
             Last_name    Text NOT NULL,
             PhoneNumber  Text NOT NULL    )";


            try
            {
                using var command = new SqliteCommand(createTableQuery, _connection);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating table: {ex.Message}");
                return false;
            };
        }

        public bool CreateTableOwner()
        {
            string createTableQuery = @"
             CREATE TABLE IF NOT EXISTS Owners (    
             Id INTEGER   PRIMARY KEY AUTOINCREMENT,
             First_name   Text NOT NULL,       
             Last_name    Text NOT NULL,
             PhoneNumber  Text NOT NULL ,
             Address      Text NOT NULL  )";


            try
            {
                using var command = new SqliteCommand(createTableQuery, _connection);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating table: {ex.Message}");
                return false;
            };
        }


        public Contact GetDataContact()
        {
            var contact = new Contact();

            Console.Write("Enter first name: ");
            contact.FirstName = Console.ReadLine();

            Console.Write("Enter last name: ");
            contact.LastName = Console.ReadLine();

            Console.Write("Enter phone number: ");
            contact.PhoneNumber = Console.ReadLine();

            return contact; 
        }

        public bool AddRowContact(Contact contact)
        {

            if (!OpenConnection()) return false;

            try
            {
                if (!CreateTableContact()) return false;

                string insertQuery = @"
                INSERT INTO contacts (First_name, Last_name, PhoneNumber)
                VALUES (@f_name, @l_name, @phone)";

                using var command = new SqliteCommand(insertQuery, _connection);
                command.Parameters.AddWithValue("@f_name", contact.FirstName);
                command.Parameters.AddWithValue("@l_name", contact.LastName);
                command.Parameters.AddWithValue("@phone", contact.PhoneNumber);

                command.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding row: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }


        public bool DeleteRowContact(int id)
        {
            if (!OpenConnection()) return false;

            try
            {
                string deleteQuery = @"DELETE FROM contacts WHERE Id = @id";


                var command = new SqliteCommand(deleteQuery, _connection);

                var rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting row: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }


        }

        public bool UpdateRowContact(int id)
        {
            throw new NotImplementedException();
        }



        public bool AddRowOwner()
        {
            throw new NotImplementedException();
        }

        public bool UpdateRowOwner(int id)
        {
            throw new NotImplementedException();
        }



        public bool GetRowContact(int id)
        {
            throw new NotImplementedException();
        }

        public bool GetRowContact(string name)
        {
            throw new NotImplementedException();
        }

        public bool GetRowOwner(int id)
        {
            throw new NotImplementedException();
        }

        public bool GetRowOwner(string name)
        {
            throw new NotImplementedException();
        }

        public bool DeleteRowOwner(int id)
        {
            throw new NotImplementedException();
        }
    }
}