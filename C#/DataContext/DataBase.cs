using Microsoft.Data.Sqlite;
using Ph_Bo_Interfaces;
using Ph_Bo_Model;
using System.Data;
using System.Data.Common;
using System.Net;

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
            ////پیاده سازی  این
            //CreateTableAsync().RunSynchronously();
            //CheckOwner();
        }

        public async Task InitAsync()
        {
            await CreateTableAsync();
           await CheckOwner();
        }

        private async Task CheckOwner()
        {
            try
            {
                if (!await OpenConnectionAsync()) return;
                var query = @"SELECT COUNT(*) FROM Owners";
                var cmd =  new SqliteCommand(query, _connection);
                var count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                HasOwner = count > 0;
            }
            catch
            {
                HasOwner = false;
            }

        }

        public async Task<bool> OpenConnectionAsync()
        {
            try
            {
              await  _connection.OpenAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening connection: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CloseConnectionAsync()
        {
            if (_connection.State == ConnectionState.Open)
            {
              await _connection.CloseAsync();

                return true;
            }
            else
                return false;
        }

        public async Task<bool> CreateTableAsync()
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
                if (!await OpenConnectionAsync()) return false;
                var command1 = new SqliteCommand(createTableQuery, _connection);
                await command1.ExecuteNonQueryAsync();
                var command2 = new SqliteCommand(createOwnerTable, _connection);
                await command2.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating table: {ex.Message}");
                return false;
            }
            finally
            {
                await CloseConnectionAsync();
            }
        }

        public async Task<bool> InsertDataAsync<T>(T entity) where T : IHuman
        {
            switch (entity)
            {
                case Contact contact:
                    return await AddRowContactAsync(contact);
                case Owner owner:
                    return await AddRowOwnerAsync(owner);
                default:
                    return false;
            }
        }

        private async Task<bool> AddRowContactAsync(Contact contact)
        {


            try
            {
                if (!await OpenConnectionAsync()) return false;
                var insertQuery =
                    @"INSERT INTO contacts (First_name, Last_name, PhoneNumber) VALUES (@f_name, @l_name, @phone)";

                var command = new SqliteCommand(insertQuery, _connection);
                command.Parameters.AddWithValue("@f_name", contact.FirstName);
                command.Parameters.AddWithValue("@l_name", contact.LastName);
                command.Parameters.AddWithValue("@phone", contact.PhoneNumber);

                await command.ExecuteNonQueryAsync();


                //for get last id 
                //last_insert_rowid() method in sqlite 
                command = new SqliteCommand("SELECT last_insert_rowid();", _connection);
                contact.Id = Convert.ToInt32(await command.ExecuteScalarAsync());

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding row: {ex.Message}");
                return false;
            }
            finally
            {
                await CloseConnectionAsync();
            }
        }

        private async Task<bool> AddRowOwnerAsync(Owner owner)
        {
            try
            {
                if (!await OpenConnectionAsync()) return false;
                var insertQuery =
                    @"INSERT INTO Owners (First_name, Last_name, PhoneNumber,Address) VALUES (@f_name,@l_name,@phone,@address)";
                var command = new SqliteCommand(insertQuery, _connection);
                command.Parameters.AddWithValue("@f_name", owner.FirstName);
                command.Parameters.AddWithValue("@l_name", owner.LastName);
                command.Parameters.AddWithValue("@phone", owner.PhoneNumber);
                command.Parameters.AddWithValue("@address", owner.Address);
                await command.ExecuteNonQueryAsync();

                command = new SqliteCommand("SELECT last_insert_rowid();", _connection);
                owner.Id = Convert.ToInt32(await command.ExecuteScalarAsync());
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding row: {ex.Message}");
                return false;
            }
            finally { await CloseConnectionAsync(); }
        }



        /// <summary>
        /// Update for database
        /// </summary>
        /// <param name="upContact"></param>
        /// <returns></returns>
        private async Task<bool> UpdateRowContactAsync(Contact upContact)
        {
            if (!await OpenConnectionAsync()) return false;

            var  updateQuery = "UPDATE Contacts  SET First_name=@first_name,Last_name=@last_name,PhoneNumber=@phone WHERE  Id=@id ";

            var cmd = new SqliteCommand(updateQuery, _connection);

            cmd.Parameters.AddWithValue("@id", upContact.Id);
            cmd.Parameters.AddWithValue("@first_name", upContact.FirstName);
            cmd.Parameters.AddWithValue("@last_name", upContact.LastName);
            cmd.Parameters.AddWithValue("@phone", upContact.PhoneNumber);
            int rowsAffected = await cmd.ExecuteNonQueryAsync();
            if (rowsAffected > 0)
                Console.WriteLine("The update was successful.");
            else
                Console.WriteLine("ID not found");

            return true;
        }
        private async Task<bool> UpdateRowOwnerAsync(Owner upOwner)
        {
            if (!await OpenConnectionAsync()) return false;

            string updateQuery = "UPDATE Owners  SET First_name=@first_name,Last_name=@last_name,PhoneNumber=@phone,Address=@address WHERE  Id=@id ";

            var cmd = new SqliteCommand(updateQuery, _connection);
            
                cmd.Parameters.AddWithValue("@id", upOwner.Id);
                cmd.Parameters.AddWithValue("@first_name", upOwner.FirstName);
                cmd.Parameters.AddWithValue("@last_name", upOwner.LastName);
                cmd.Parameters.AddWithValue("@phone", upOwner.PhoneNumber);
                cmd.Parameters.AddWithValue("@address", upOwner.Address);
                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                if (rowsAffected > 0)
                    Console.WriteLine("The update was successful.");
                else
                    Console.WriteLine("ID not found");
            
            return true;
        }

        public async Task<T> UpdateDatabaseAsync<T>(T entity) where T : IHuman
        {
            if (entity is Contact contact)
            {
                bool success = await UpdateRowContactAsync(contact);
                if (success)
                    return entity;
            }
            else if (entity is Owner owner)
            {
                bool success = await UpdateRowOwnerAsync(owner);
                if (success) return entity;

            }
            throw new NotImplementedException($"Type {typeof(T)} not supported.");
        }
        public async Task<bool> DeleteRowAsync(int id)
        {
            try
            {
                if (!await OpenConnectionAsync()) return false;
                var deleteQuery = $"DELETE FROM contacts WHERE Id = @id ";

                var command = new SqliteCommand(deleteQuery, _connection);
                command.Parameters.AddWithValue("@id", id);
                var rowsAffected = await command.ExecuteNonQueryAsync();
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
            finally
            {
                await CloseConnectionAsync();
            }
        }
        public async Task DisplayAllAsync(string tableName)
        {
            try
            {
                if (!await OpenConnectionAsync()) return;

                var query = $"SELECT * FROM {tableName}";
                var cmd = new SqliteCommand(query, _connection);
                var reader = await cmd.ExecuteReaderAsync();
                Console.WriteLine("A list of  ");
                Console.WriteLine("-----------------");
                Console.WriteLine();

                while (await reader.ReadAsync())
                {
                    Console.WriteLine(
                        $"id:{reader["Id"]},FirstName:{reader["First_name"]},LastName:{reader["Last_name"]},Phone:{reader["PhoneNumber"]}\t");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error displaying data from table '{tableName}': {ex.Message}");
            }
            finally
            {
                await CloseConnectionAsync();
            }
        }



        /// <summary>
        /// Getting information by making a bet
        /// </summary>
        public async Task<T> GetElementByIdAsync<T>(int id) where T : IHuman
        {
            if (typeof(T) == typeof(Contact))
            {
                var contact = await GetRowContactAsync(id);
                return (T)contact;
            }
            if (typeof(T) == typeof(Owner))
            {
                var owner = await GetRowOwnerAsync(id);
                return (T)owner;
            }
            throw new NotImplementedException($"Type {typeof(T)} not supported.");
        }

        public async Task<List<T>> GetElementByNameAsync<T>(string name) where T : IHuman
        {
            if (typeof(T) == typeof(Contact))
            {
                var contacts = await GetRowContactAsync(name);
                return contacts.OfType<T>().ToList();
            }
            // return await GetRowContact(name).Cast<T>().ToList();
            if (typeof(T) == typeof(Owner))
            {
                var owners = await GetRowOwnerAsync(name);
                return owners.OfType<T>().ToList();
            }

            return new List<T>();
        }

        private async Task<IContact> GetRowContactAsync(int id)
        {
            try
            {
                if (!await OpenConnectionAsync()) return null;
                string selectQuery = "SELECT * FROM  contacts WHERE Id=@id";
                var command = new SqliteCommand(selectQuery, _connection);

                //To send value securely
                command.Parameters.AddWithValue("@id", id);


                var reader = await command.ExecuteReaderAsync();


                if (await reader.ReadAsync())
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
                await CloseConnectionAsync();
            }
        }

        private async Task<List<IContact>> GetRowContactAsync(string name)
        {
            if (!await OpenConnectionAsync())
            {
                Console.WriteLine("Connection failed");
                return new List<IContact>();
            }

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
            finally
            {
                await CloseConnectionAsync();
            }
        }
        private async Task<IOwner> GetRowOwnerAsync(int id)
        {
            try
            {
                if (!await OpenConnectionAsync()) return null;
                string selectQuery = "SELECT * FROM  Owners WHERE Id=@id";
                var command = new SqliteCommand(selectQuery, _connection);

                //To send value securely
                command.Parameters.AddWithValue("@id", id);


                var reader = await command.ExecuteReaderAsync();


                if (await reader.ReadAsync())
                {
                    var owner = new Owner()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
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
                await CloseConnectionAsync();
            }
        }
        private async Task<List<IOwner>> GetRowOwnerAsync(string name)
        {
            var results = new List<IOwner>();
            try
            {
                if (!await OpenConnectionAsync())
                {
                    Console.WriteLine("Connection could not be opened.");
                    return new List<IOwner>();
                }



                var selectQuery = "SELECT * FROM  Owners WHERE  First_name LIKE  @name  ";
                var command = new SqliteCommand(selectQuery, _connection);

                command.Parameters.AddWithValue("@name", "%" + name + "%");
                //درواقع جایگری میشه با @name بالا در کوئری 

                var reader = await command.ExecuteReaderAsync();


                while (await reader.ReadAsync())
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
                await CloseConnectionAsync();
            }

        }
    }
}