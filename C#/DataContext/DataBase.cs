using Microsoft.Data.Sqlite;
using Ph_Bo_Interfaces;
using Ph_Bo_Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Reflection;
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
             PhoneNumber  Text NOT NULL);";

            var createOwnerTable = @"
             CREATE TABLE IF NOT EXISTS Owners (
             Id INTEGER PRIMARY KEY AUTOINCREMENT,
             First_name TEXT NOT NULL,
             Last_name TEXT NOT NULL,
             PhoneNumber TEXT NOT NULL,
             Address TEXT NOT NULL
             );";



            try
            {

                new SqliteCommand(createTableQuery, _connection).ExecuteNonQuery();
                new SqliteCommand(createOwnerTable, _connection).ExecuteNonQuery();
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


                var insertQuery = @"INSERT INTO contacts (First_name, Last_name, PhoneNumber) VALUES (@f_name, @l_name, @phone)";

                var command = new SqliteCommand(insertQuery, _connection);
                command.Parameters.AddWithValue("@f_name", contact.FirstName);
                command.Parameters.AddWithValue("@l_name", contact.LastName);
                command.Parameters.AddWithValue("@phone", contact.PhoneNumber);

                command.ExecuteNonQuery();


                //for get last id 
                //last_insert_rowid() method in sqlite 
                command = new SqliteCommand("SELECT last_insert_rowid();", _connection);
                contact.Id = Convert.ToInt32(command.ExecuteScalar());

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

                var insertQuery = @"INSERT INTO Owners (First_name, Last_name, PhoneNumber,Address) VALUES (@f_name,@l_name,@phone,@address)";
                var command = new SqliteCommand(insertQuery, _connection);
                command.Parameters.AddWithValue("@f_name", owner.FirstName);
                command.Parameters.AddWithValue("@l_name", owner.LastName);
                command.Parameters.AddWithValue("@phone", owner.PhoneNumber);
                command.Parameters.AddWithValue("@address", owner.Address);
                command.ExecuteNonQuery();
                command = new SqliteCommand("SELECT last_insert_rowid();", _connection);
                owner.Id = Convert.ToInt32(command.ExecuteScalar());
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


        public bool DeleteRow(string tableName, int id)
        {
            if (!OpenConnection()) return false;

            try
            {
                var deleteQuery = @$"DELETE FROM {tableName} WHERE Id = @id";


                var command = new SqliteCommand(deleteQuery, _connection);
                command.Parameters.AddWithValue("@id", id);
                var rowsAffected = command.ExecuteNonQuery();


                if (rowsAffected > 0)
                {
                    Console.WriteLine($"{tableName} with ID {id} was deleted.");
                    return true;
                }
                else
                {
                    Console.WriteLine($"No {tableName} found with ID {id}.");
                    return false;
                }


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
        //public bool DeleteRowContact(int id)
        //{
        //    if (!OpenConnection()) return false;

        //    try
        //    {
        //        var deleteQuery = @"DELETE FROM contacts WHERE Id = @id";


        //        var command = new SqliteCommand(deleteQuery, _connection);
        //        command.Parameters.AddWithValue("@id", id);
        //        var rowsAffected = command.ExecuteNonQuery();


        //        if (rowsAffected > 0)
        //        {
        //            Console.WriteLine($" with ID {id} was deleted.");
        //            return true;
        //        }
        //        else
        //        {
        //            Console.WriteLine($"No owner found with ID {id}.");
        //            return false;
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error deleting row: {ex.Message}");
        //        return false;
        //    }
        //    finally
        //    {
        //        CloseConnection();
        //    }


        //}
        //public bool DeleteRowOwner(int id)
        //{
        //    if (!OpenConnection()) return false;

        //    try
        //    {
        //        var deleteQuery = @"DELETE FROM Owners  WHERE Id = @id";


        //        var command = new SqliteCommand(deleteQuery, _connection);
        //        command.Parameters.AddWithValue("@id", id);
        //        var rowsAffected = command.ExecuteNonQuery();


        //        if (rowsAffected > 0)
        //        {
        //            Console.WriteLine($"Owner with ID {id} was deleted.");
        //            return true;
        //        }
        //        else
        //        {
        //            Console.WriteLine($"No owner found with ID {id}.");
        //            return false;
        //        }



        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error deleting row: {ex.Message}");
        //        return false;
        //    }
        //    finally
        //    {
        //        CloseConnection();
        //    }
        //}


        public bool UpdateRowContact(Contact upContact)
        {
            if (!OpenConnection()) return false;

            string updateQuery = "UPDATE Contacts  SET First_name=@first_name,Last_name=@last_name,PhoneNumber=@phone WHERE  Id=@id ";

            using (var cmd = new SqliteCommand(updateQuery, _connection))
            {
                cmd.Parameters.AddWithValue("@id", upContact.Id);
                cmd.Parameters.AddWithValue("@first_name", upContact.FirstName);
                cmd.Parameters.AddWithValue("@last_name", upContact.LastName);
                cmd.Parameters.AddWithValue("@phone", upContact.PhoneNumber);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                    Console.WriteLine("The update was successful.");
                else
                    Console.WriteLine("ID not found");
            }
            return true;
        }


        public bool UpdateRowOwner(int id)
        {
            throw new NotImplementedException();
        }







        public void DisplayAll(string tableName)
        {
            if (OpenConnection())
            {

                var query = $"SELECT * FROM {tableName}";
                var cmd = new SqliteCommand(query, _connection);
                SqliteDataReader reader = cmd.ExecuteReader();
                Console.WriteLine("A list of contacts "); Console.WriteLine("-----------------"); Console.WriteLine();

                while (reader.Read())
                {
                    Console.WriteLine($"id:{reader["Id"]},FirstName:{reader["First_name"]},LastName:{reader["Last_name"]},Phone:{reader["PhoneNumber"]}\t");


                }
            }
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
        public List<T> GetElementByName<T>(string name) where T : IHuman
        {
            if (typeof(T) == typeof(Contact))
                return GetRowContact(name).Cast<T>().ToList();
            else if (typeof(T) == typeof(Owner))
                return GetRowOwner(name).Cast<T>().ToList();
            return new List<T>();

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
                        Id = Convert.ToInt32(reader["Id"]),
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
                string selectQuery = "SELECT * FROM contacts WHERE First_name LIKE @name ";
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





        public bool IsTableEmpty()
        {
            string OwnerQuery = @"SELECT COUNT(*) FROM Owners";
            var cmd = new SqliteCommand(OwnerQuery, _connection);
            
                int count = Convert.ToInt32(cmd.ExecuteScalar());
            return count == 0;


        }


    }
}