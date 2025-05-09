using C_.DataContext;
using Microsoft.Data.Sqlite;
using Ph_Bo_Interfaces;
using Ph_Bo_Model;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace Ph_Bo;

class Phone_book
{
    private static DataBase _dataBase;
    private static bool _appRunning = true;
    private static void UpdateContact()
    {
        Console.WriteLine("enter  id :");
        var idU = int.Parse(Console.ReadLine() ?? "0");
        Console.WriteLine("enter new  first name : ");
        var newFirstName = Console.ReadLine() ?? "";
        Console.WriteLine("enter new last name : ");
        var newLastName = Console.ReadLine() ?? "";
        Console.WriteLine("enter new phone : ");
        var newPhone = Console.ReadLine() ?? "";
        var updated = new Contact()
        {
            Id = idU,
            FirstName = newFirstName,
            LastName = newLastName,
            PhoneNumber = newPhone
        };

    }
    private static void Display()
    {
        _dataBase.DisplayAll("contacts");
        Console.WriteLine("enter Enter to continue");
        Console.ReadLine();
    }
    private static void AddOwner()
    {
        if (_dataBase.HasOwner) return;
        var flag = true;
        Console.WriteLine("It is the first time you are using this phone book");
        while (flag)
        {
            Console.Write("Enter your First Name: ");
            var name = Console.ReadLine();
            Console.Write("Enter your Last Name: ");
            var lastName = Console.ReadLine();
            Console.Write("Enter your Phone Number: ");
            var phone = Console.ReadLine();
            Console.Write("Enter your Address: ");
            var address = Console.ReadLine();
            var owner = new Owner()
            {
                FirstName = name ?? "",
                LastName = lastName ?? "",
                PhoneNumber = phone ?? "",
                Address = address ?? ""
            };
            flag = !_dataBase.InsertData<Owner>(owner);
            Console.Clear();
        }
    }

    private static void AddContact()
    {
        var flag = true;
        while (flag)
        {
            Console.Write("Enter First Name: ");
            var name = Console.ReadLine();
            Console.Write("Enter Last Name: ");
            var lastName = Console.ReadLine();
            Console.Write("Enter Phone Number: ");
            var phone = Console.ReadLine();
            var contact = new Contact()
            {
                FirstName = name ?? "",
                LastName = lastName ?? "",
                PhoneNumber = phone ?? "",
            };
            Console.WriteLine(_dataBase.InsertData<Contact>(contact) ? "Contact added" : "Contact not added");
            Console.Write("Do you want to continue ? y/n");
            flag = Console.ReadKey().Key == ConsoleKey.Y;
            Console.Clear();
        }
    }

    private static void DeleteContact()
    {
        Console.WriteLine("Enter contact id: ");
        var id = int.Parse(Console.ReadLine() ?? "0");
        _dataBase.DeleteRow(id);
    }

    private static void Exit()
    {
        _appRunning = false;
        _dataBase.CloseConnection();
    }

    private static void GetContacts()
    {
        Console.WriteLine("Enter contact id or name: ");
        var userInput = Console.ReadLine();
        if (int.TryParse(userInput, out var id))
        {
            var contact = _dataBase.GetElementById<Contact>(id);
            Console.WriteLine($"id {contact.Id}  name {contact.FirstName + " " + contact.LastName} phone {contact.PhoneNumber}");
        }
        else
        {
            var contacts = _dataBase.GetElementByName<Contact>(userInput);
            foreach (var contact in contacts)
            {
                Console.WriteLine($"id {contact.Id}  name {contact.FirstName + " " + contact.LastName} phone {contact.PhoneNumber}");
            }
        }
        Console.Write("press enter to continue");
        Console.ReadLine();
    }
    private static void Menu()
    {
        Console.WriteLine("Please select the desired option.");
        Console.WriteLine("1.Add contact");
        Console.WriteLine("2.Show list");
        Console.WriteLine("3.Edit contact");
        Console.WriteLine("4.Search");
        Console.WriteLine("5.Delete contact");
        Console.WriteLine("6.Exit");
        try
        {
            var choice = int.Parse(Console.ReadLine() ?? "0");
            switch (choice)
            {
                case 1:
                    AddContact();
                    break;
                case 2:
                    Display();
                    break;
                case 3:
                    UpdateContact();
                    break;
                case 4:
                    GetContacts();
                    break;
                case 5:
                    DeleteContact();
                    break;
                case 6:
                    Exit();
                    break;
            }
        }
        catch
        {
            // ignored
        }

        Console.Clear();
    }

    static void Main(string[] args)
    {
        _dataBase = new DataBase("phone_book.db");
        AddOwner();
        while (_appRunning)
        {
            Menu();
        }
    }
}