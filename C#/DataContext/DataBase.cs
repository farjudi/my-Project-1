using Microsoft.Data.Sqlite;
using Ph_Bo_Interfaces;
using Ph_Bo_Model;
using System.Data;

namespace C_.DataContext
{
    public class DataBase : IDataBase
    {
        /// <summary>
        ///  Represents a connection to an SQLite database.  
        /// </summary>
        private readonly SqliteConnection _connection;
        public bool HasOwner { get; private set; }

        public DataBase(string dbName)
        {
            var path = Path.Combine(Environment.CurrentDirectory, dbName);
            _connection = new SqliteConnection($"Data Source={path}");
            OpenConnection();
            CreateTable();
            CheckOwner();
        }

        private void CheckOwner()
        {
            try
            {
                var query = @"SELECT COUNT(*) FROM Owners";
                var cmd = new SqliteCommand(query, _connection);
                var count = Convert.ToInt32(cmd.ExecuteScalar());
                HasOwner = count > 0;
            }
            catch
            {
                HasOwner = false;
            }
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
        }

        public bool InsertData<T>(T entity) where T : IHuman
        {
            switch (entity)
            {
                case Contact contact:
                    return AddRowContact(contact);
                case Owner owner:
                    return AddRowOwner(owner);
                default:
                    return false;
            }
        }

        private bool AddRowContact(Contact contact)
        {
            try
            {
                var insertQuery =
                    @"INSERT INTO contacts (First_name, Last_name, PhoneNumber) VALUES (@f_name, @l_name, @phone)";

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
        }

        private bool AddRowOwner(Owner owner)
        {
            try
            {
                var insertQuery =
                    @"INSERT INTO Owners (First_name, Last_name, PhoneNumber,Address) VALUES (@f_name,@l_name,@phone,@address)";
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
        }

        public bool DeleteRow(int id)
        {
            try
            {
                var deleteQuery = $"DELETE FROM contacts WHERE Id = '{id}';";
                var command = new SqliteCommand(deleteQuery, _connection);
                var rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine($"contact with ID {id} was deleted.");
                    return true;
                }
                else
                {
                    Console.WriteLine($"No contact found with ID {id}.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting row: {ex.Message}");
                return false;
            }
        }
        public void DisplayAll(string tableName)
        {
            var query = $"SELECT * FROM {tableName}";
            var cmd = new SqliteCommand(query, _connection);
            var reader = cmd.ExecuteReader();
            Console.WriteLine("A list of contacts ");
            Console.WriteLine("-----------------");
            Console.WriteLine();

            while (reader.Read())
            {
                Console.WriteLine(
                    $"id:{reader["Id"]},FirstName:{reader["First_name"]},LastName:{reader["Last_name"]},Phone:{reader["PhoneNumber"]}\t");
            }
        }

        /// <summary>
        /// Getting information by making a bet
        /// </summary>
        public T GetElementById<T>(int id) where T : IHuman
        {
            if (typeof(T) == typeof(Contact))
                return (T)GetRowContact(id);
            if (typeof(T) == typeof(Owner))
                return (T)GetRowOwner(id);
            throw new NotImplementedException();
        }

        public List<T> GetElementByName<T>(string name) where T : IHuman
        {
            if (typeof(T) == typeof(Contact))
                return GetRowContact(name).Cast<T>().ToList();
            if (typeof(T) == typeof(Owner))
                return GetRowOwner(name).Cast<T>().ToList();
            return new List<T>();
        }

        private IContact GetRowContact(int id)
        {
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
                var selectQuery = "SELECT * FROM contacts WHERE First_name LIKE @name OR Last_name LIKE @name";
                var command = new SqliteCommand(selectQuery, _connection);
                command.Parameters.AddWithValue("@name", "%" + name + "%");
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
                return new List<IContact>(); //Empty list 
            }
        }
        private IOwner GetRowOwner(int id)
        {
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
                var selectQuery = "SELECT * FROM  Owners WHERE  First_name LIKE  @name  ";
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
        }
    }
}