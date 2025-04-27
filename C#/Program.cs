using C_.DataContext;


using Microsoft.Data.Sqlite;
using Ph_Bo_Model;
using System.Numerics;
using System.Xml.Linq;

namespace Ph_Bo;
class Phone_book
{
    static void Main(string[] args)
    {
        DataBase db = new DataBase("phone_book.db");
        db.OpenConnection();
        db.CreateTableContact();


        Console.WriteLine("Welcome to our program.");
        while (true)
        {
            {
                Console.WriteLine("Please select the desired option.");
                Console.WriteLine("1.Add contact OR owner  ");
                Console.WriteLine("2.Show list");
                Console.WriteLine("3.Edit audience");
                Console.WriteLine("4.Search by name");
                Console.WriteLine("5.DELEDT ");
                Console.WriteLine("6.Exit");
                int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        Console.WriteLine(" select to add for contact or owner writ the 'c' or 'o'  :");
                        var select = char.Parse(Console.ReadLine());
                        if (select == 'c')
                        {
                            Console.Write("Enter first name : ");
                            string firstName = Console.ReadLine();
                            Console.Write("Enter last name : ");
                            string lastName = Console.ReadLine();
                            Console.Write("Enter phone: ");
                            string phone = Console.ReadLine();
                            db.AddRowContact(new Contact { FirstName = firstName, LastName = lastName, PhoneNumber = phone });
                            Console.WriteLine("Contact added successfully!");
                        }
                        else if (select == 'o')
                        {
                            Console.Write("Enter first name : ");
                            string firstName = Console.ReadLine();
                            Console.Write("Enter last name : ");
                            string lastName = Console.ReadLine();
                            Console.Write("Enter phone: ");
                            string phone = Console.ReadLine();
                            Console.WriteLine("Enter address");
                            string address = Console.ReadLine();
                            db.AddRowOwner(new Owner { FirstName = @firstName, LastName = @lastName, PhoneNumber = @phone, Address = @address });
                            Console.WriteLine("Owner added successfully!");

                        }
                        break;
                    case 2:
                        Console.WriteLine("show contact or owner  write oen of them :");
                        var choes = Console.ReadLine();
                        if (choes== "contact") 
                        { 
                        Console.WriteLine("Search by name or Id \t Write your choice ");
                        var selectUser = Console.ReadLine();
                        if (selectUser == "name")
                        {
                            Console.WriteLine("Enter the name for Search ");
                            var name= Console.ReadLine();
                            db.GetRowContact(name);

                        }
                        else if (selectUser == "Id")
                        {
                            Console.WriteLine("enter the id  for show ");
                            int id = int.Parse(Console.ReadLine());
                            db.GetRowContact(id);
                        }
                        }
                        else if(choes== "owner")
                        {
                            Console.WriteLine("Search by name or Id \t Write your choice ");
                            var selectUser = Console.ReadLine();
                            if (selectUser == "name")
                            {
                                Console.WriteLine("Enter the name for Search ");
                                var name = Console.ReadLine();
                                db.GetRowOwner(name);

                            }
                            else if (selectUser == "Id")
                            {
                                Console.WriteLine("enter the id  for show ");
                                int id = int.Parse(Console.ReadLine());
                                db.GetRowOwner(id);
                            }

                        }
                        break;
                    case 3:

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
            
            //else
            //{
            //    Console.WriteLine("Enter owner information.\n ");
            //    Console.Write("name ::  ");
            //    string name = Console.ReadLine();
            //    Console.Write("phone number :: ");
            //    string phone = Console.ReadLine();

            //    string insertQuery = @"INSERT INTO Owners (OwnerName,OwnerContact) VALUES (@Owner_name,@Owner_phone) ";
            //    using (var cmd = new SqliteCommand(insertQuery, connection))
            //    {
            //        cmd.Parameters.AddWithValue("@Owner_name", name);
            //        cmd.Parameters.AddWithValue("@Owner_phone", phone);

            //        cmd.ExecuteNonQuery();
            //    }
            //    Console.WriteLine("added successfully");
            //}


        }
    }
}
//Does our notebook have an owner or not?
//static bool CheckOwnerContact(SqliteConnection connection)
//{
//    string OwnerQuery = @"SELECT COUNT(*) FROM Owners";
//    using (var cmd = new SqliteCommand(OwnerQuery, connection))
//    {
//        int count = Convert.ToInt32(cmd.ExecuteScalar());
//        return count == 0;
//    }

//}

////Show all lists
//static void DisplayContacts(SqliteConnection connection)
//{
//    string selectQuery = "SELECT * FROM contacts";
//    using (var cmd = new SqliteCommand(selectQuery, connection))
//    using (SqliteDataReader reader = cmd.ExecuteReader())
//    {
//        Console.WriteLine("A list of contacts "); Console.WriteLine("-----------------"); Console.WriteLine();
//        while (reader.Read())
//        {
//            Console.WriteLine($"id:{reader["Id"]},FirstName:{reader["First_name"]},LastName:{reader["Last_name"]},Phone:{reader["phone"]}");

//        }
//        Console.WriteLine("-----------------"); Console.WriteLine();
//    }
//}


////Update using ID
//static void UpdateContact(SqliteConnection connection)
//{
//    Console.WriteLine("enter  id :");
//    int id = int.Parse(Console.ReadLine());
//    Console.WriteLine("enter new  first name : ");
//    string newFirst_name = Console.ReadLine();

//    Console.WriteLine("enter new last name : ");
//    string newLast_name = Console.ReadLine();
//    Console.WriteLine("enter new phone : ");
//    string newPhone = Console.ReadLine();

//    string updateQuery = "UPDATE Contacts  SET First_name=@first_name,Last_name=@last_name,Phone=@phone WHERE  Id=@id ";

//    using (var cmd = new SqliteCommand(updateQuery, connection))
//    {
//        cmd.Parameters.AddWithValue("@id", id);
//        cmd.Parameters.AddWithValue("@first_name", newFirst_name);
//        cmd.Parameters.AddWithValue("@last_name", newLast_name);
//        cmd.Parameters.AddWithValue("@phone", newPhone);
//        int rowsAffected = cmd.ExecuteNonQuery();
//        if (rowsAffected > 0)
//            Console.WriteLine("The update was successful.");
//        else
//            Console.WriteLine("ID not found");

//    }

//}

//}

//}
