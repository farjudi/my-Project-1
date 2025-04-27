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


        public bool AddRowContact(Contact contact)
        {

            if (!OpenConnection()) return false;

            try
            {
                if (!CreateTableContact()) return false;

                var insertQuery = @"
                INSERT INTO contacts (First_name, Last_name, PhoneNumber)
                VALUES (@f_name, @l_name, @phone)";

                var command = new SqliteCommand(insertQuery, _connection);
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
        public bool AddRowOwner(Owner owner)
        {
            if (!OpenConnection()) return false;
            try
            {
                if (!CreateTableOwner()) return false;
                var insertQuery = $"INSERT INTO Owners(First_name, Last_name, PhoneNumber,Address) VALUES (@f_name,@l_name,@phone,@address)";
                var command = new SqliteCommand(insertQuery, _connection);
                command.Parameters.AddWithValue("@f_name", owner.FirstName);
                command.Parameters.AddWithValue("@l_name", owner.LastName);
                command.Parameters.AddWithValue("@phone", owner.PhoneNumber);
                command.Parameters.AddWithValue("@address", owner.Address);
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
        public bool DeleteRowOwner(int id)
        {
            throw new NotImplementedException();
        }
        public bool UpdateRowContact(int id)
        {
            throw new NotImplementedException();
        }


        public bool UpdateRowOwner(int id)
        {
            throw new NotImplementedException();
        }



        public bool GetRowContact(int id)
        {
            if (!OpenConnection()) return false;

            try
            {
                if (!CreateTableContact()) return false;
                string selectQuery = "SELECT * FROM  contacts WHERE Id=@id";
                var command = new SqliteCommand(selectQuery, _connection);

                //To send value securely
                command.Parameters.AddWithValue("@id", id);



                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        Console.WriteLine($"id: {reader["Id"]}, FirstName: {reader["First_name"]}, LastName: {reader["Last_name"]}, Phone: {reader["PhoneNumber"]}");
                    }
                }



                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching row: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool GetRowContact(string name)
        {
            if (!OpenConnection()) return false;

            try
            {
                if (!CreateTableContact()) return false;
                string selectQuery = "SELECT * FROM  contacts WHERE  First_name LIKE  @name  ";
                var command = new SqliteCommand(selectQuery, _connection);

                command.Parameters.AddWithValue("@name", "%" + name + "%");
                //درواقع جایگری میشه با @name بالا در کوئری 




                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        Console.WriteLine($"id: {reader["Id"]}, FirstName: {reader["First_name"]}, LastName: {reader["Last_name"]}, Phone: {reader["PhoneNumber"]}");
                    }
                }



                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching row: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool GetRowOwner(int id)
        {
            if (!OpenConnection()) return false;

            try
            {
                if (!CreateTableContact()) return false;
                string selectQuery = "SELECT * FROM  Owners WHERE Id=@id";
                var command = new SqliteCommand(selectQuery, _connection);

                //To send value securely
                command.Parameters.AddWithValue("@id", id);



                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        Console.WriteLine($"id: {reader["Id"]}, FirstName: {reader["First_name"]}, LastName: {reader["Last_name"]}, Phone: {reader["PhoneNumber"]},Address{reader["Address"]}");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching row: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool GetRowOwner(string name)
        {
            if (!OpenConnection()) return false;

            try
            {
                if (!CreateTableContact()) return false;
                string selectQuery = "SELECT * FROM  Owners WHERE  First_name LIKE  @name  ";
                var command = new SqliteCommand(selectQuery, _connection);

                command.Parameters.AddWithValue("@name", "%" + name + "%");
                //درواقع جایگری میشه با @name بالا در کوئری 




                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        Console.WriteLine($"id: {reader["Id"]}, FirstName: {reader["First_name"]}, LastName: {reader["Last_name"]}, Phone: {reader["PhoneNumber"]},Address{reader["Address"]}");
                    }
                }



                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching row: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }


    }
}