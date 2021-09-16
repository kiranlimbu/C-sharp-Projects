using System;

public class View
{
    Data data = new Data();
    User user = new User();
    Logic logic = new Logic();
    public void login()
    {
        bool isLoggedIn = false;
        int loginAttempt = 0;
        string un = "x"; // temp username variable
        bool nameMatch = false;

        Console.WriteLine("\n------ Login Screen ------\n");

        try
        {
            while (!isLoggedIn)
            {
                Console.Write("Username: "); // input user name
                string username = Console.ReadLine();
                
                Console.Write("Pin: "); // input user pin
                string userpin = Console.ReadLine();

                // encrypt input
                user.UserName = logic.encryption(username);
                user.Pin = logic.encryption(userpin);

                // check if there is a matching name
                // use it to disable account if needed
                if (logic.nameMatch(user.UserName))
                {
                    un = user.UserName;
                    nameMatch = true;
                }

                // Verify Login info
                // get user role info
                if (logic.verifyLogin(user, out string role))
                {
                    // check if the account is active
                    if (logic.isActive(user.UserName))
                    {
                        user.Role = role;
                        isLoggedIn = true;
                    }
                    else
                    {
                        Console.WriteLine("\nYour account is disabled! Please contact your admin.\n");
                        break;
                    }   
                }
                else
                {
                    loginAttempt++;
                    // allow only 3 attempt to login
                    if (loginAttempt < 3)
                    {
                        Console.WriteLine("\n------ Incorrect Username or Pin! ------\n" + 
                        "Please try again.\n");
                    }
                    else // beyond 3 attempts
                    {
                        if (nameMatch) // if we know the username
                        {
                            logic.disableAcc(un); // disable account
                            Console.WriteLine("\nYou missed 3 login attempts. Your acctount has been disabled.\n" +
                            "Please contact your admin.\n");
                            break;
                        }
                        else // we don't know the user name
                        {
                            Console.WriteLine("\nYou missed 3 login attempts. Please try later!\n");
                            break;
                        }
                    }
                }
            }// end of while loop

            if (user.Role == "Admin")
            {
                adminMenu();
            }
            else if (user.Role == "Customer")
            {
                customerMenu();
            }
        }
        catch (Exception)
        {
            Console.WriteLine("\nPlease try later!\n");
        }
    }

    public void adminMenu()
    {
        Console.Clear();
        Console.WriteLine("\n------ Admin Menu ------\n");
        Console.WriteLine("1.  Create New Account\n" +
                          "2.  Delete Existing Account\n" +
                          "3.  Update account information\n" +
                          "4.  Search for Account\n" +
                          "5.  View Reports\n" +
                          "6.  Exit\n");
        
        Console.Write("\nEnter your option: ");
        string option = Console.ReadLine(); // need safer way to parse

        if (option == "1")
        {
            Console.Clear();
            Console.WriteLine("\n------ Create New Account ------\n");
            logic.createAccount();
        }
        else if (option == "2")
        {
            Console.Clear();
            Console.WriteLine("\n------ Delete Account ------\n");
            logic.deleteAccount();
        }
        else if (option == "3")
        {
            Console.Clear();
            Console.WriteLine("\n------ Update Account Information ------\n");
            logic.updateAccount();
        }
        else if (option == "4")
        {
            Console.Clear();
            Console.WriteLine("\n------ Search for Account ------\n");
            logic.SearchAccount();
        }
        else if (option == "5")
        {
            Console.Clear();
            Console.WriteLine("\n------ View Reports ------\n");
            logic.ViewReports();
        }
        else if (option == "6")
        {
            Console.Clear();
            login();
        }
        else
        {
            Console.WriteLine("Invalid Input! Press enter to continue.");
            Console.ReadLine();
            adminMenu();
        }

        adminORlogin:
        {
            Console.WriteLine("\nDo you want another operation (y/n)? ");
            string input = Console.ReadLine();
            if (input.ToLower().StartsWith("y"))
            {
                adminMenu();
            }
            else if (input.ToLower().StartsWith("n"))
            {
                login();
            }
            else
            {
                Console.WriteLine("\nInvalid input!\n");
                goto adminORlogin;
            }
        }

    }

    // Customer Menu
    public void customerMenu()
    {
        Console.Clear();
        Console.WriteLine("\n------ Customer Menu ------\n");
        Console.WriteLine("1.  Withdraw Cash\n" +
                          "2.  Cash transfer\n" +
                          "3.  Deposit Cash\n" +
                          "4.  Display Balance\n" +
                          "5.  Exit\n");
        
        Console.Write("\nEnter your option: ");
        string option = Console.ReadLine(); // need safer way to parse

        if (option == "1")
        {
            Console.Clear();
            Console.WriteLine("\n------ Withdraw Cash ------\n");
            logic.CashWithDraw(user.UserName);
        }
        else if (option == "2")
        {
            Console.Clear();
            Console.WriteLine("\n------ Cash Transfer ------\n");
            logic.CashTransfer(user.UserName);
        }
        else if (option == "3")
        {
            Console.Clear();
            Console.WriteLine("\n------ Cash Deposit ------\n");
            logic.CashDeposite(user.UserName);
        }
        else if (option == "4")
        {
            Console.Clear();
            Console.WriteLine("\n------ Display Balance ------\n");
            logic.DisplayBalance(user.UserName);
        }
        else if (option == "5")
        {
            Console.Clear();
            login();
        }
        else
        {
            Console.WriteLine("Invalid Input! Press enter to continue.");
            Console.ReadLine();
            customerMenu();
        }

        custORlogin:
        {
            Console.WriteLine("\nDo you want another operation (y/n)? ");
            string input = Console.ReadLine();
            if (input.ToLower().StartsWith("y"))
            {
                customerMenu();
            }
            else if (input.ToLower().StartsWith("n"))
            {
                login();
            }
            else
            {
                Console.WriteLine("\nInvalid input!\n");
                goto custORlogin;
            }
        }
    }
}