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




        /// <summary>
        /// We want our phone book to be the owner. If it doesn't exist, we will definitely add it.
        /// </summary>
        if (!db.IsTableEmpty())
        {

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
                            var _tableName = Console.ReadLine();
                            db.DisplayAll(_tableName);

                            break;
                        case 3:
                            //مالک مونده 
                            Console.WriteLine("enter  id :");
                            int serchId = int.Parse(Console.ReadLine());
                            Console.WriteLine("enter new  first name : ");
                            string fName = Console.ReadLine();

                            Console.WriteLine("enter new last name : ");
                            string lName = Console.ReadLine();
                            Console.WriteLine("enter new phone : ");
                            string phoneN = Console.ReadLine();
                            var contactList = new Contact
                            {
                                Id = serchId,
                                FirstName = fName,
                                LastName = lName,
                                PhoneNumber = phoneN
                            };
                            if (db.UpdateRowContact(contactList))
                            {
                                Console.WriteLine("update the contcat ");
                            }
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
            }
        }

        else
        {
            Console.WriteLine("Enter owner information.\n ");


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
            if (db.AddRowOwner(owner))
                Console.WriteLine("added successfully");
        }


    }
}





