

using Microsoft.Data.Sqlite;
using System.Numerics;

namespace Ph_Bo;
class Phone_book
{
    static void Main(string[] args)
    {
        //create database
        using (var connection = new SqliteConnection("Data Source=contacts.db;"))
        {
            connection.Open();

            //if there is not table
            string createTableQuery = @"
             CREATE TABLE IF NOT EXISTS contacts (    
             Id INTEGER  PRIMARY KEY AUTOINCREMENT,
             First_name  Text NOT NULL,       
              Last_name  Text NOT NULL,
              Phone  Text NOT NULL    )";

            using (var cmd = new SqliteCommand(createTableQuery, connection))
            {
                cmd.ExecuteNonQuery();
            }


            //main menu

            while (true)
            {
                Console.WriteLine("Welcome to our program.");
                Console.WriteLine("Please select the desired option.");
                Console.WriteLine("1.Add contact  ");
                Console.WriteLine("2.Show list");
                Console.WriteLine("3.Edit audience");
                Console.WriteLine("4.Search by name");
                Console.WriteLine("5.Exit");
                int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        AddContact(connection);
                        break;
                    case 2:
                        DisplayContacts(connection);
                        break;
                    case 3:
                        UpdateContact(connection);
                        break;
                    case 4:
                        break;
                    case 5:
                        return;
                    default:
                        Console.WriteLine("Choose the correct option.");
                        break;


                }
            }
        }
    }

    //Add member
    static void AddContact(SqliteConnection connection)
    {
        Console.Write("enter the first name    ::    ");
        string first_name = Console.ReadLine();
        Console.Write("enter the last name     ::    ");
        string last_name = Console.ReadLine();
        Console.Write("enter the phone number  ::    ");
        string phone = Console.ReadLine();



        string insertQuery = "INSERT INTO contacts(First_name,Last_name,Phone) VALUES (@f_name,@l_name,@phone) ";

        using (var cmd = new SqliteCommand(insertQuery, connection))
        {
            cmd.Parameters.AddWithValue("@f_name", first_name);
            cmd.Parameters.AddWithValue("@l_name", last_name);
            cmd.Parameters.AddWithValue("@phone", phone);

            cmd.ExecuteNonQuery();
        }
        Console.WriteLine("added successfully");

    }
    //Show all lists
    static void DisplayContacts(SqliteConnection connection)
    {
        string selectQuery = "SELECT * FROM contacts";
        using (var cmd = new SqliteCommand(selectQuery, connection))
        using (SqliteDataReader reader = cmd.ExecuteReader())
        {
            Console.WriteLine("A list of contacts ");
            while (reader.Read())
            {
                Console.WriteLine($"id:{reader["Id"]},FirstName:{reader["First_name"]},LastName:{reader["Last_name"]},Phone:{reader["phone"]}");
            }
        }
    }


    //Update using ID
    static void UpdateContact(SqliteConnection connection)
    {
        Console.WriteLine("enter  id :");
        int id = int.Parse(Console.ReadLine());
        Console.WriteLine("enter new  first name : ");
        string newFirst_name = Console.ReadLine();

        Console.WriteLine("enter new last name : ");
        string newLast_name = Console.ReadLine();
        Console.WriteLine("enter new phone : ");
        string newPhone = Console.ReadLine();

        string updateQuery = "UPDATE Contacts  SET First_name=@first_name,Last_name=@last_name,Phone=@phone WHERE  Id=@id ";

        using (var cmd = new SqliteCommand(updateQuery, connection))
        {
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@first_name", newFirst_name);
            cmd.Parameters.AddWithValue("@last_name", newLast_name);
            cmd.Parameters.AddWithValue("@phone", newPhone);
            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected > 0)
                Console.WriteLine("The update was successful.");
            else
                Console.WriteLine("ID not found");

        }

    }

}
