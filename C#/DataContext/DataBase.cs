using Microsoft.Data.Sqlite;
using Ph_Bo_Interfaces;
using Ph_Bo_Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
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

            CreateTable();

            Console.WriteLine("DB Path: " + path);

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

        public bool CreateTable()
        {
            if (!OpenConnection()) return false;

            var createTableQuery = @"
             CREATE TABLE IF NOT EXISTS contacts (    
             Id INTEGER   PRIMARY KEY AUTOINCREMENT,
             First_name   Text NOT NULL,       
             Last_name    Text NOT NULL,
             PhoneNumber  Text NOT NULL);

             CREATE TABLE IF NOT EXISTS Owners (
             Id INTEGER PRIMARY KEY AUTOINCREMENT,
             First_name TEXT NOT NULL,
             Last_name TEXT NOT NULL,
             PhoneNumber TEXT NOT NULL,
             Address TEXT NOT NULL
             );
";



            try
            {

                var command = new SqliteCommand(createTableQuery, _connection);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating table: {ex.Message}");
                return false;
            }
            finally
            {
                CloseConnection();
            }


        }


        public bool AddRowContact(Contact contact)
        {
            if (!OpenConnection()) return false;
            try
            {


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

                var insertQuery = @"INSERT INTO Owners(First_name, Last_name, PhoneNumber,Address) VALUES (@f_name,@l_name,@phone,@address)";
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



        /// <summary>
        /// Getting information by making a bet
        /// </summary>

        public T GetElementById<T>(int id) where T : IHuman
        {
            if (typeof(T) == typeof(Contact))
                return (T)GetRowContact(id);
            else if (typeof(T) == typeof(Owner))
                return (T)GetRowOwner(id);
            throw new NotImplementedException();



        }
        public List<T> GetElementByName<T>(string name) where T: IHuman
        {
            if (typeof(T) == typeof(Contact))
                return GetRowContact(name) as List<T> ?? new List<T>();
            else if (typeof(T) == typeof(Owner))
                return GetRowOwner(name) as List<T> ?? new List<T>();

            return null;

        }

        private IContact GetRowContact(int id)
        {


            if (!OpenConnection()) return null;
            //if (!OpenConnection()) return new Contact();
            //// چرا از اومد نمونه ساخت ! سازنده خالی  کال شد 

            try
            {

                string selectQuery = "SELECT * FROM  contacts WHERE Id=@id";
                var command = new SqliteCommand(selectQuery, _connection);

                //To send value securely
                command.Parameters.AddWithValue("@id", id);


                var reader = command.ExecuteReader();


                if (reader.Read())
                {
                    var contact = new Contact
                    {
                        FirstName = reader["First_name"]?.ToString() ?? string.Empty,
                        LastName = reader["Last_name"]?.ToString() ?? string.Empty,
                        PhoneNumber = reader["PhoneNumber"]?.ToString() ?? string.Empty
                    };

                    return contact;
                }

                return null;


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching row: {ex.Message}");
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }

        private List<IContact> GetRowContact(string name)
        {
           
            if (!OpenConnection())
            {
                Console.WriteLine("Connection failed");
                return new List<IContact>();
            }

            //  if (!OpenConnection()) return new Contact();

            var results = new List<IContact>();
            try
            {
                string selectQuery = "SELECT * FROM contacts WHERE First_name LIKE @name COLLATE NOCASE";
                var command = new SqliteCommand(selectQuery, _connection);
                command.Parameters.AddWithValue("@name", "%" + name + "%");
                //درواقع جایگری میشه با @name بالا در کوئری 


                var reader = command.ExecuteReader();


                while (reader.Read())
                {
                    var contact = new Contact
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        FirstName = reader["First_name"].ToString() ?? string.Empty,
                        LastName = reader["Last_name"].ToString() ?? string.Empty,
                        PhoneNumber = reader["PhoneNumber"].ToString() ?? string.Empty
                    };
                    results.Add(contact);
                }

                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching row: {ex.Message}");
                return new List<IContact>();//Empty list 
            }
            finally
            {
                CloseConnection();
            }
        }

        private IOwner GetRowOwner(int id)
        {
            if (!OpenConnection()) return null;

            try
            {

                string selectQuery = "SELECT * FROM  Owners WHERE Id=@id";
                var command = new SqliteCommand(selectQuery, _connection);

                //To send value securely
                command.Parameters.AddWithValue("@id", id);



                var reader = command.ExecuteReader();




                if (reader.Read())
                {

                    var owner = new Owner()
                    {
                        FirstName = reader["First_name"]?.ToString() ?? string.Empty,
                        LastName = reader["Last_name"]?.ToString() ?? string.Empty,
                        PhoneNumber = reader["PhoneNumber"]?.ToString() ?? string.Empty,
                        Address = reader["Address"].ToString() ?? string.Empty

                        //اگر مقدار داشت تبیدل کنه به استرینگ اگه نداشت رشته خالی برگدونه 

                    };
                    return owner;


                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching row: {ex.Message}");
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }

        private List<IOwner> GetRowOwner(string name)
        {
            //if (!OpenConnection()) return null;
            if (!OpenConnection())
            {
                Console.WriteLine("Connection could not be opened.");
                return new List<IOwner>(); 
            }

            var results = new List<IOwner>();
            try
            {

                string selectQuery = "SELECT * FROM  Owners WHERE  First_name LIKE  @name  ";
                var command = new SqliteCommand(selectQuery, _connection);

                command.Parameters.AddWithValue("@name", "%" + name + "%");
                //درواقع جایگری میشه با @name بالا در کوئری 




                var reader = command.ExecuteReader();


                while (reader.Read())
                {
                    var owner = new Owner
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        FirstName = reader["First_name"].ToString() ?? string.Empty,
                        LastName = reader["Last_name"].ToString() ?? string.Empty,
                        PhoneNumber = reader["PhoneNumber"]?.ToString() ?? string.Empty,
                        Address = reader["Address"]?.ToString() ?? string.Empty

                    };
                    results.Add(owner);
                }

                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching row: {ex.Message}");
                return new List<IOwner>();
            }
            finally
            {
                CloseConnection();
            }
        }


    }
}