using BusinessLayer;
using BusinessLayer.DTOs;
using DataLayer;
using Entities;
using System;

class Program
{
  static async Task  Main()
    {
        using (var context = new UserContext())
        {
            IUserRepository userRepository = new UserRepository(context);
            IUserService userService = new UserService(userRepository);

            while (true)
            {
                Console.WriteLine("Enter your command:");
                string? input = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Command cannot be empty.");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }

                var parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string command = parts[0].ToLower();

                try
                {
                    switch (command)
                    {
                        case "register":
                            {
                               await RegisterAsync(parts, userService);
                                break;
                            }

                        case "login":
                            {
                                await LoginAsync(parts, userService);
                                break;
                            }

                        case "change":
                            {
                                await ChangeStatusAsync(parts, userService);
                                break;
                            }

                        case "search":
                            {
                                await SearchAsync(parts, userService);
                                break;
                            }

                        case "logout":
                            {
                                Logout(userService);
                                break;
                            }
                        case "changepassword":
                            {
                                await ChangePasswordAsync(parts, userService);
                                break;
                            }

                        default:
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Unknown command.");
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }

            }
        }
    }
    static void ShowHelpMenu()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\nAvailable Commands:");
        Console.WriteLine("Usage: change --status <available|not available>");
        Console.WriteLine("Usage: search --username <username>");
        Console.WriteLine("Usage: changepassword --old <oldPassword> --new <newPassword>");
        Console.ForegroundColor = ConsoleColor.White;
    }
    static async Task RegisterAsync(string[] parts, IUserService userService)
    {
        if (parts.Length < 5)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Usage: register --username <username> --password <password>");
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }
        var username = parts[2];
        var password = parts[4];
        var result =await userService.RegisterUserAsync(username, password);
        if (result.Success)
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        Console.WriteLine(result.Message);
        Console.ForegroundColor = ConsoleColor.White;

    }

    static async Task LoginAsync(string[] parts, IUserService userService)
    {
        if (parts.Length < 5)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Usage: login --username <username> --password <password>");
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }
        var username = parts[2];
        var password = parts[4];
        var result =await userService.LoginAsync(username, password);

        if (result.Success)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(result.Message);
            ShowHelpMenu();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(result.Message);
        }
        Console.ForegroundColor = ConsoleColor.White;
    }

    static async Task ChangeStatusAsync(string[] parts, IUserService userService)
    {
        if (parts.Length < 3)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Usage: change --status <available|not available>");
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }
        var status = parts[2].ToLower();
        bool isAvailable = status == "available";

        var result =await userService.ChangeStatusAsync(isAvailable);
        if (result.Success)
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        Console.WriteLine(result.Message);
        Console.ForegroundColor = ConsoleColor.White;
    }

    static async Task SearchAsync(string[] parts, IUserService userService)
    {
        if (parts.Length < 3)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Usage: search --username <username>");
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }
        var usernamePrefix = parts[2];
        var result = await userService.GetAllUsersByUserNameAsync(usernamePrefix);

        if (result.Success)
        {
            int index = 1;
            foreach (var user in result.Data)
            {
                Console.WriteLine($"{index++}- {user.Username} | status: {(user.IsAvailable ? "available" : "not available")}");
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(result.Message);
            Console.ForegroundColor = ConsoleColor.White;

        }
    }

    static void Logout(IUserService userService)
    {
        var result =  userService.Logout();
        if (result.Success)
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        Console.WriteLine(result.Message);
        Console.ForegroundColor = ConsoleColor.White;
    }

    static async Task ChangePasswordAsync(string[] parts, IUserService userService)
    {
        if (parts.Length < 5)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Usage: changepassword --old <oldPassword> --new <newPassword>");
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }
        var oldPassword = parts[2];
        var newPassword = parts[4];
        var result =await userService.ChangePasswordAsync(oldPassword, newPassword);

        if (result.Success)
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        Console.WriteLine(result.Message);
        Console.ForegroundColor = ConsoleColor.White;
    }


}
