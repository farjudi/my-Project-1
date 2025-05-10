using C_.DataContext;
using Microsoft.Data.Sqlite;
using Ph_Bo_Interfaces;
using Ph_Bo_Model;
using System.Net;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace Ph_Bo;

class Phone_book
{
    private static DataBase _dataBase;
    private static bool _appRunning = true;
    private  static async Task UpdateAsync()
    {
        Console.WriteLine("update the contact OR owner write");

        var userSelect = Console.ReadLine();
        if (userSelect == "contact")
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
                FirstName = newFirstName ?? "",
                LastName = newLastName ?? "",
                PhoneNumber = newPhone ?? ""
            };

          await  _dataBase.UpdataDatabaseAsync(updated);
        }
        else if(userSelect == "owner")
        {
            Console.WriteLine("enter  id :");
            var idU = int.Parse(Console.ReadLine() ?? "0");
            Console.WriteLine("enter new  first name : ");
            var newFirstName = Console.ReadLine() ?? "";
            Console.WriteLine("enter new last name : ");
            var newLastName = Console.ReadLine() ?? "";
            Console.WriteLine("enter new phone : ");
            var newPhone = Console.ReadLine() ?? "";
            Console.WriteLine("enter the Address");
            var newAddress = Console.ReadLine() ?? "";
            var updated = new Owner()
            {
                Id = idU,
                FirstName = newFirstName ?? "",
                LastName = newLastName ?? "",
                PhoneNumber = newPhone ?? "",
                Address= newAddress ?? ""
            };

            _dataBase.UpdataDatabaseAsync(updated);
        }
    }
    private static async Task DisplayAsync()
    {
        await _dataBase.DisplayAllAsync("contacts");
        Console.WriteLine("enter Enter to continue");
        Console.ReadLine();
    }
    private static async Task AddOwnerAsync()
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
            flag = !await _dataBase.InsertDataAsync<Owner>(owner);
            Console.Clear();
        }
    }

    private static async Task AddContact()
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
            Console.WriteLine(await _dataBase.InsertDataAsync<Contact>(contact) ? "Contact added" : "Contact not added");
            Console.Write("Do you want to continue ? y/n");
            flag = Console.ReadKey().Key == ConsoleKey.Y;
            Console.Clear();
        }
    }

    private static async Task DeleteContact()
    {
        Console.WriteLine("Enter contact id: ");
        var id = int.Parse(Console.ReadLine() ?? "0");
        await _dataBase.DeleteRowAsync(id);
    }

    private static void Exit()
    {
        _appRunning = false;

    }

    private static async Task GetContacts()
    {
        Console.WriteLine("Enter contact id or name: ");
        var userInput = Console.ReadLine();
        if (int.TryParse(userInput, out var id))
        {
            var contact = await _dataBase.GetElementByIdAsync<Contact>(id);
            Console.WriteLine($"id {contact.Id}  name {contact.FirstName + " " + contact.LastName} phone {contact.PhoneNumber}");
        }
        else
        {
            var contacts = await _dataBase.GetElementByNameAsync<Contact>(userInput);
            foreach (var contact in contacts)
            {
                Console.WriteLine($"id {contact.Id}  name {contact.FirstName + " " + contact.LastName} phone {contact.PhoneNumber}");
            }
        }
        Console.Write("press enter to continue");
        Console.ReadLine();
    }
    private static async Task Menu()
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
                    await AddContact();
                    break;
                case 2:
                   await DisplayAsync();
                    break;
                case 3:
                  await UpdateAsync();
                    break;
                case 4:
                    await GetContacts();
                    break;
                case 5:
                    await DeleteContact();
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

    /// <summary>
    ///  این متد رو چک کن نباید تسک باشه 
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    static async Task Main(string[] args)
    {
        _dataBase = new DataBase("phone_book.db");
        await AddOwnerAsync();
        while (_appRunning)
        {
            Menu();
        }
    }
}