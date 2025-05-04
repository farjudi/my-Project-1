using C_.DataContext;
using Microsoft.Data.Sqlite;
using Ph_Bo_Interfaces;
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


        Console.WriteLine("Welcome to our program.");
        while (true)
        {
            {
                Console.WriteLine("Please select the desired option.");
                Console.WriteLine("1.Add contact OR owner  ");
                Console.WriteLine("2.Show list");
                Console.WriteLine("3.Edit audience");
                Console.WriteLine("4.Search by name");
                Console.WriteLine("5.Delete ");
                Console.WriteLine("6.Exit");
                int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        Console.WriteLine(" select to add for contact or owner writ the 'c' or 'o'  :\n");
                        var select = char.Parse(Console.ReadLine());
                        if (select == 'c')
                        {
                            Console.Write("Enter first name :   ");
                            var firstName = Console.ReadLine();
                            Console.Write("Enter last name :   ");
                            var lastName = Console.ReadLine();
                            Console.Write("Enter phone:   ");
                            var phone = Console.ReadLine();
                            var contact = new Contact
                            {
                                FirstName = firstName,
                                LastName = lastName,
                                PhoneNumber = phone
                            };

                            db.AddRowContact(contact);
                            Console.WriteLine($"Saved contact with Id = {contact.Id}");

                            //Console.WriteLine("Contact added successfully!");
                        }
                        else if (select == 'o')
                        {
                            Console.Write("Enter first name :  ");
                            var firstName = Console.ReadLine();
                            Console.Write("Enter last name : ");
                            var lastName = Console.ReadLine();
                            Console.Write("Enter phone: ");
                            var phone = Console.ReadLine();
                            Console.WriteLine("Enter address:  ");
                            var address = Console.ReadLine();
                            var owner = new Owner
                            {
                                FirstName = firstName,
                                LastName = lastName,
                                PhoneNumber = phone,
                                Address = address
                            };
                            db.AddRowOwner(owner);
                            Console.WriteLine($"Saved owner with Id = {owner.Id}");
                            //Console.WriteLine("Owner added successfully!");

                        }
                        break;
                    case 2:
                        Console.WriteLine("enter the table name      contacts or  Owners::");
                        var _tableName= Console.ReadLine();
                        db.DisplayAll(_tableName);

                        break;
                    case 3:
                        Console.WriteLine("enter  id :");
                        var idU = int.Parse(Console.ReadLine());
                        Console.WriteLine("enter new  first name : ");
                        var newFirstName = Console.ReadLine();

                        Console.WriteLine("enter new last name : ");
                        var newLastName = Console.ReadLine();
                        Console.WriteLine("enter new phone : ");
                        string newPhone = Console.ReadLine();

                        break;
                    case 4:
                        Console.WriteLine("show contact or owner  write oen of them :");

                        var user_choice = Console.ReadLine();

                        Console.WriteLine("Search by name or Id \t Write your choice ");
                        var selectUser = Console.ReadLine();
                        if (selectUser == "name")
                        {
                            Console.WriteLine("Enter the name for Search ");
                            var name = Console.ReadLine();
                            if (user_choice == "contact")
                            {
                                var contact = db.GetElementByName<Contact>(name);
                                //for check  
                                Console.WriteLine(contact.Count);
                                if (contact == null || contact.Count == 0)
                                {
                                    Console.WriteLine("No contact found.");
                                }
                                else
                                {
                                    foreach (var c in contact)
                                        Console.WriteLine($"ID: {c.Id}, Name: {c.FirstName} {c.LastName}, Phone: {c.PhoneNumber}");
                                }
                            }
                            else if (user_choice == "owner")
                            {
                                var owners = db.GetElementByName<Owner>(name);
                                if (owners == null || owners.Count == 0)
                                {
                                    Console.WriteLine("No owners found.");
                                }
                                else
                                {
                                    foreach (var o in owners)
                                        Console.WriteLine($"ID: {o.Id}, Name: {o.FirstName} {o.LastName}, Phone: {o.PhoneNumber}, Address: {o.Address}");
                                }

                            }


                        }
                        else if (selectUser == "Id")
                        {
                            Console.WriteLine("enter the id  for show ");
                            int id = int.Parse(Console.ReadLine());
                            if (user_choice == "contact")
                            {
                                var contact = db.GetElementById<Contact>(id);
                                if (contact != null)
                                    Console.WriteLine($"ID: {id}, Name: {contact.FirstName} {contact.LastName}, Phone: {contact.PhoneNumber}");
                                else
                                    Console.WriteLine("Contact not found.");
                            }

                            else if (user_choice == "owner")
                            {
                                var owner = db.GetElementById<Owner>(id);
                                if (owner != null)
                                    Console.WriteLine($"ID: {id}, Name: {owner.FirstName} {owner.LastName}, Phone: {owner.PhoneNumber}, Address: {owner.Address}");
                                else
                                    Console.WriteLine("Owner not found.");
                            }

                        }

                        break;
                    case 5:
                        Console.WriteLine("enter the id for Delete ");
                        var U_id = int.Parse(Console.ReadLine());
                        Console.WriteLine("enter the table name  Owners or contacts: ");
                        var tableName = Console.ReadLine();
                        if (db.DeleteRow(tableName, U_id))
                        {
                            Console.Write("delete successful ");
                        }
                        break;
                    case 6:
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
