

using Microsoft.Data.Sqlite;
using System.Numerics;
using System.Xml.Linq;

namespace Ph_Bo;
class Phone_book
{
    static void Main(string[] args)
    {
        //connecting to database
        using (var connection = new SqliteConnection("Data Source=contacts.db;"))
        {
            connection.Open();

            //Creating a table for contacts
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

            //Creating a table for the owner
            string createOwnerTableQuery = @"CREATE TABLE IF NOT EXISTS Owners
            (Id INTEGER PRIMARY KEY AUTOINCREMENT,
            OwnerName  Text NOT NULL,
            OwnerContact  Text NOT NULL  )";
            using (var cmd = new SqliteCommand(createOwnerTableQuery, connection))
            {
                cmd.ExecuteNonQuery();
            }



            while (true)
            {
                bool checkOwner = CheckOwnerContact(connection);

                Console.WriteLine("Welcome to our program.");
                if (checkOwner != true)
                {


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
                            SearchByName(connection);
                            break;
                        case 5:
                            return;
                        default:
                            Console.WriteLine("Choose the correct option.");
                            break;


                    }
                }
                else
                {
                    Console.WriteLine("Enter owner information.\n ");
                    Console.Write("name ::  ");
                    string name = Console.ReadLine();
                    Console.Write("phone number :: ");
                    string phone = Console.ReadLine();

                    string insertQuery = @"INSERT INTO Owners (OwnerName,OwnerContact) VALUES (@Owner_name,@Owner_phone) ";
                    using (var cmd = new SqliteCommand(insertQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@Owner_name", name);
                        cmd.Parameters.AddWithValue("@Owner_phone", phone);

                        cmd.ExecuteNonQuery();
                    }
                    Console.WriteLine("added successfully");
                }


            }
        }
    }
    //Does our notebook have an owner or not?
    static bool CheckOwnerContact(SqliteConnection connection)
    {
        string OwnerQuery = @"SELECT COUNT(*) FROM Owners";
        using (var cmd = new SqliteCommand(OwnerQuery, connection))
        {
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            return count == 0;
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
            Console.WriteLine("A list of contacts "); Console.WriteLine("-----------------"); Console.WriteLine();
            while (reader.Read())
            {
                Console.WriteLine($"id:{reader["Id"]},FirstName:{reader["First_name"]},LastName:{reader["Last_name"]},Phone:{reader["phone"]}");

            }
            Console.WriteLine("-----------------"); Console.WriteLine();
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
    //Find by name
    static void SearchByName(SqliteConnection connection)
    {
        Console.Write("enter the user name:\t   ");
        string user_name = Console.ReadLine();


        string search_Query = "SELECT * FROM contacts WHERE   First_name LIKE  @user_name";

        using (var cmd = new SqliteCommand(search_Query, connection))
        {
            cmd.Parameters.AddWithValue("@user_name", "%" + user_name + "%");

            using (var reader = cmd.ExecuteReader())
            {
                Console.WriteLine(" ");
                bool found = false;
                while (reader.Read())
                {
                    Console.WriteLine($"id:{reader["Id"]}, first name {reader["First_name"]}, last name {reader["Last_name"]}, phone number {reader["Phone"]}");
                    found = true;
                }
                if (!found)
                    Console.WriteLine("No results found.");
            }


        }


    }

}
