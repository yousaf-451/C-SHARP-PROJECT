using System;
using System.IO;
using System.Collections.Generic;

class BakerySystem
{
	static void Main()
	{
		char choice;

		do
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("\n=============================");
			Console.WriteLine("   BAKERY MANAGEMENT SYSTEM");
			Console.WriteLine("=============================");
			Console.ResetColor();

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("a) Admin Login");
			Console.WriteLine("b) User Signup");
			Console.WriteLine("c) User Login");
			Console.WriteLine("d) Exit");
			Console.ResetColor();

			Console.Write("Enter choice: ");
			choice = Console.ReadKey().KeyChar;
			Console.WriteLine();

			switch (choice)
			{
				case 'a': AdminLogin(); break;
				case 'b': UserSignup(); break;
				case 'c': UserLogin(); break;
				case 'd':
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("Thank you for using the system!");
					Console.ResetColor();
					break;
				default:
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Invalid choice!");
					Console.ResetColor();
					break;
			}

		} while (choice != 'd');
	}

	// ================= ADMIN =================

	static void AdminLogin()
	{
		Console.Write("Admin Username: ");
		string user = Console.ReadLine();
		Console.Write("Admin Password: ");
		string pass = Console.ReadLine();

		if (user == "yousaf" && pass == "aeiou451")
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Admin Login Successful!");
			Console.ResetColor();

			char ch;
			do
			{
				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.WriteLine("\n--- ADMIN PANEL ---");
				Console.ResetColor();

				Console.WriteLine("a) Add Item");
				Console.WriteLine("b) Show Items");
				Console.WriteLine("c) Update Item");
				Console.WriteLine("d) Delete Item");
				Console.WriteLine("e) Logout");

				Console.Write("Enter choice: ");
				ch = Console.ReadKey().KeyChar;
				Console.WriteLine();

				switch (ch)
				{
					case 'a': AddItem(); break;
					case 'b': ShowItems(); break;
					case 'c': UpdateItem(); break;
					case 'd': DeleteItem(); break;
				}

			} while (ch != 'e');
		}
		else
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Invalid Admin Login!");
			Console.ResetColor();
		}
	}

	static int GenerateItemID()
	{
		int lastID = 100;

		if (File.Exists("items.txt"))
		{
			foreach (var line in File.ReadAllLines("items.txt"))
			{
				var parts = line.Split('|');
				int id = int.Parse(parts[0]);
				if (id > lastID)
					lastID = id;
			}
		}

		return lastID + 1;
	}

	static void AddItem()
	{
		int id = GenerateItemID();

		Console.Write("Enter Item Name: ");
		string name = Console.ReadLine();

		Console.Write("Enter Price: ");
		int price = int.Parse(Console.ReadLine());

		Console.Write("Enter Stock: ");
		int stock = int.Parse(Console.ReadLine());

		File.AppendAllText("items.txt", $"{id}|{name}|{price}|{stock}\n");

		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine($"Item Added Successfully! Item ID: {id}");
		Console.ResetColor();
	}

	static void ShowItems()
	{
		if (!File.Exists("items.txt"))
		{
			Console.WriteLine("No items available.");
			return;
		}

		Console.WriteLine("\nID | Name | Price | Stock");

		foreach (var line in File.ReadAllLines("items.txt"))
		{
			var parts = line.Split('|');
			Console.WriteLine($"{parts[0]} | {parts[1]} | Rs {parts[2]} | {parts[3]}");
		}
	}

	static void UpdateItem()
	{
		if (!File.Exists("items.txt")) return;

		var lines = new List<string>(File.ReadAllLines("items.txt"));

		Console.Write("Enter Item ID to update: ");
		int targetID = int.Parse(Console.ReadLine());

		for (int i = 0; i < lines.Count; i++)
		{
			var parts = lines[i].Split('|');

			if (int.Parse(parts[0]) == targetID)
			{
				Console.Write("Enter new Name: ");
				string name = Console.ReadLine();

				Console.Write("Enter new Price: ");
				int price = int.Parse(Console.ReadLine());

				Console.Write("Enter new Stock: ");
				int stock = int.Parse(Console.ReadLine());

				lines[i] = $"{parts[0]}|{name}|{price}|{stock}";
				File.WriteAllLines("items.txt", lines);

				Console.WriteLine("Item Updated Successfully!");
				return;
			}
		}

		Console.WriteLine("Item ID not found!");
	}

	static void DeleteItem()
	{
		if (!File.Exists("items.txt")) return;

		var lines = new List<string>(File.ReadAllLines("items.txt"));

		Console.Write("Enter Item ID to delete: ");
		int targetID = int.Parse(Console.ReadLine());

		lines.RemoveAll(line => line.StartsWith(targetID + "|"));
		File.WriteAllLines("items.txt", lines);

		Console.WriteLine("Item Deleted Successfully!");
	}

	// ================= USER =================

	static void UserSignup()
	{
		Console.Write("Enter Username: ");
		string user = Console.ReadLine();

		Console.Write("Enter Password (6-8 chars): ");
		string pass = Console.ReadLine();

		File.AppendAllText("users.txt", $"{user} {pass}\n");

		Console.WriteLine("Signup Successful!");
	}

	static void UserLogin()
	{
		if (!File.Exists("users.txt"))
		{
			Console.WriteLine("No users found.");
			return;
		}

		Console.Write("Username: ");
		string user = Console.ReadLine();
		Console.Write("Password: ");
		string pass = Console.ReadLine();

		foreach (var line in File.ReadAllLines("users.txt"))
		{
			var parts = line.Split(' ');
			if (parts[0] == user && parts[1] == pass)
			{
				Console.WriteLine("Login Successful!");
				BuyItems();
				return;
			}
		}

		Console.WriteLine("Invalid Login!");
	}

	static void BuyItems()
	{
		if (!File.Exists("items.txt")) return;

		var lines = new List<string>(File.ReadAllLines("items.txt"));
		int total = 0;
		char more;

		do
		{
			Console.WriteLine("\nID | Name | Price | Stock");

			foreach (var line in lines)
			{
				var parts = line.Split('|');
				Console.WriteLine($"{parts[0]} | {parts[1]} | {parts[2]} | {parts[3]}");
			}

			Console.Write("Enter Item ID: ");
			int id = int.Parse(Console.ReadLine());

			Console.Write("Enter Quantity: ");
			int qty = int.Parse(Console.ReadLine());

			for (int i = 0; i < lines.Count; i++)
			{
				var parts = lines[i].Split('|');

				if (int.Parse(parts[0]) == id)
				{
					int price = int.Parse(parts[2]);
					int stock = int.Parse(parts[3]);

					if (qty <= stock)
					{
						stock -= qty;
						total += price * qty;

						lines[i] = $"{parts[0]}|{parts[1]}|{price}|{stock}";
						Console.WriteLine($"Added to cart: {parts[1]} x {qty}");
					}
					else
					{
						Console.WriteLine("Not enough stock!");
					}
				}
			}

			Console.Write("Buy more? (y/n): ");
			more = Console.ReadKey().KeyChar;
			Console.WriteLine();

		} while (more == 'y');

		File.WriteAllLines("items.txt", lines);
		File.AppendAllText("receipt.txt", $"TOTAL BILL = Rs {total}\n");

		Console.WriteLine($"\nTOTAL BILL = Rs {total}");
		Console.WriteLine("Thank you for your purchase!");
	}
}