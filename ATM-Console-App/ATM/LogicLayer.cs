using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

public class Logic
{
    Data data = new Data();

    // verify Login and pass their role information
    public bool verifyLogin(User user, out string role)
    {
        var result = data.isInFile(user, out string roleType);
        role = roleType;
        return result;
    }

    // Encryption ROT13
    public string encryption(string inputValue)
    {
        string code = "";

        foreach (char c in inputValue)
        {
            if (c >= 'A' && c <= 'Z')
            {
                if (c > 'M') code += Convert.ToChar(c - 13); // M is the 13th character
                else code += Convert.ToChar(c + 13);
            }
            else if (c >= 'a' && c <= 'z')
            {
                if (c > 'm') code += Convert.ToChar(c - 13);
                else code += Convert.ToChar(c + 13);
            }
            else if (c >= '0' && c <= '9')
            {
                code += (9 - Char.GetNumericValue(c)); // when I append to code, it becomes string
            }
        }

        return code;
    }

    // check if name is valid
    public bool isValidName(string username)
    {
        if (username.Length < 5)
        {
            Console.WriteLine("You need atleast 5 characters!");
            return false;
        }

        foreach (char c in username)
        {
            if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')) 
                continue;
            else
            {
                Console.WriteLine("Only alphabets accepted!");
                return false;
            }
        }
        return true;
    }


    // check if uer pin is valid
    public bool isValidPin(string userpin)
    {
        if (userpin.Length != 4)
        {
            Console.WriteLine("You need 4 digit pin!");
            return false;
        }

        foreach (char c in userpin)
        {
            if (c >= '0' && c <= '9') 
                continue;
            else
            {
                Console.WriteLine("Only numbers accepted!");
                return false;
            }
        }
        return true;
    }

    // check if accrount is active
    public bool isActive(string username)
    {
        // get user info
        User personnel = data.getCustomer(username);
        // check status
        if (personnel is not null)
        {
            return personnel.Status == "Active";
        }
        return false;
        
    }

    // check if there is matching name
    public bool nameMatch(string username)
    {
        if (username != "")
        {
            User personnel = data.getCustomer(username);
            if (personnel is not null)
            {
                return personnel.UserName == username;
            }        
        }
        return false;
    }

    // disable account
    public void disableAcc(string username)
    {
        // get user info
        User customer = data.getCustomer(username);

        // change status
        customer.Status = "Disabled";

        // update the status in file
        data.updateInFile(customer);
    }

    // Get User choice between 1 and 2
    public string getUserSelection(string input, string option1, string option2)
    {
        if (input == "1") return option1;
        else if (input == "2") return option2;
        return "";
    }

    // print account detail
    public void printAccDetail(User customer)
    {
        Console.WriteLine($"\nAccount No: {customer.AccountNo}\n" +
                          $"Type: {customer.AccountType}\n" +
                          $"Balance: {customer.Balance}\n\n" +
                          $"1. Name: {customer.FirstName + " " + customer.LastName}\n" +
                          $"2. Status: {customer.Status}");
    }

    // get user name
    public string getUserName()
    {
        string userN="";
        bool state = true;

        while (state)
        {
            Console.Write("\nUsername: ");
            string tempName = Console.ReadLine();
            
            if (isValidName(tempName))
            {
                // Encrypt username
                userN = encryption(tempName);
                return userN;
            }
        }
        return userN;
    }

    // get user pin
    public string getUserPin()
    {
        string userP = "";
        bool state = true;

        while(state)
        {
            // input user pin
            Console.Write("Pin: ");
            string tempPin = Console.ReadLine();

            if (isValidPin(tempPin))
            {
                // Encrypt pin
                userP = encryption(tempPin);
                return userP;
            }
        }
        return userP;
    }

    // get account number
    public int getAccountNo()
    {
        int userAccount = 0;
        bool state = true;

        while (state)
        {
            Console.Write("Enter the Account Number: ");
            try
            {
                userAccount = Convert.ToInt32(Console.ReadLine());
                return userAccount;
            }
            catch (Exception)
            {
                Console.WriteLine("\nInvalid Input! Please try again.\n");
            }
        }

        return userAccount;
    }

    // create new account
    public void createAccount()
    {
        User user = new User();

        // get username
        bool state = true;
        while (state)
        {
            // input username
            string username = getUserName();
            // check if the username is taken
            if (data.getCustomer(username) is null)
            {
                user.UserName = username;
                break;
            }
            else
            {
                Console.WriteLine("Username already exists!");
            }
        }
        // get user pin
        user.Pin = getUserPin();

        // input user first name
        Console.Write("First Name: ");
        user.FirstName = Console.ReadLine();

        // input user last name
        Console.Write("Last Name: ");
        user.LastName = Console.ReadLine();

        // input user role
        Console.Write("User Role (1 = Admin, 2 = Customer): ");
        user.Role = getUserSelection(Console.ReadLine(), "Admin", "Customer");
        
        // enter following field only for customer
        if (user.Role == "Customer")
        {
            // input acc type
            Console.Write("Account Type (1 = Checking Account, 2 = Saving account): ");
            user.AccountType = getUserSelection(Console.ReadLine(), "Checking", "Saving");

            // input balance
            inputBalance:
            {
                Console.Write("Account Balance: ");
                double userB;
                bool success = double.TryParse(Console.ReadLine(), out userB);
                if (success) user.Balance = userB;
                else
                {
                    Console.WriteLine("Invalid input!");
                    goto inputBalance;
                }
            }
        }

        // input status
        Console.Write("User Status (1 = Active, 2 = Disabled): ");
        user.Status = getUserSelection(Console.ReadLine(), "Active", "Disabled");

        // account number should be automatically assigned
        user.AccountNo = data.getAccCount()+1;

        // ADD to file
        data.addToFile(user);

        Console.WriteLine("\nAccount Successfully Created!\n" + 
                          $"The account number assigned to this account is: {user.AccountNo}");
        
    }

    // delete Account
    public void deleteAccount()
    {
        int userA = getAccountNo();

        // check if record exists
        if (data.isInFile(userA, out User customer))
        {
            deleteConfirm:
            {
                Console.Write($"\nYou are about to delete the account held by {customer.FirstName + " " + customer.LastName}.\n" +
                               "Do you want to continue (y/n)? ");
                string input = Console.ReadLine();

                if (input.ToLower().StartsWith("y"))
                {
                    data.deleteFromFile(userA);
                    Console.WriteLine("\nAccount deleted Successfully.\n");
                }
                else if (!input.ToLower().StartsWith("n"))
                {
                    Console.WriteLine("\nInvalid input! Please try again.");
                    goto deleteConfirm;
                }
            }
        }
        else
        {
            Console.WriteLine($"Account number {userA} does not exist!");
        }
    }

    // update account information
    public void updateAccount()
    {
        // get account number
        int accNumber = getAccountNo();

        // verify if the account exists
        // fetch customer account
        if (data.isInFile(accNumber, out User customer))
        {
            // display the account detail
            printAccDetail(customer);
            Console.WriteLine($"3. UserName: {encryption(customer.UserName)}\n" + // decrypt and display
                              $"4. Pin: {encryption(customer.Pin)}\n" + // decrypt and display
                              $"5. Submit\n");
                              
            updateOptions:
            {
                Console.Write("\nWhat would you like to update? ");
                string userinput = Console.ReadLine();

                if (userinput == "1")
                {
                    // input user first name
                    Console.Write("First Name: ");
                    customer.FirstName = Console.ReadLine();

                    // input user last name
                    Console.Write("Last Name: ");
                    customer.LastName = Console.ReadLine();
                    goto updateOptions;
                }
                else if (userinput == "2")
                {
                    // input status
                    Console.Write("User Status (1 = Active, 2 = Disabled): ");
                    customer.Status = getUserSelection(Console.ReadLine(), "Active", "Disabled");
                    goto updateOptions;

                }
                else if (userinput == "3")
                {
                    string username;
                    bool state = true;
                    while (state)
                    {
                        // input username
                        username = getUserName();
                        // check if the username is taken
                        if (!data.isInFile(username))
                        {
                            customer.UserName = username;
                            goto updateOptions;
                        }
                        else
                        {
                            Console.WriteLine("Username already exists!");
                        }
                    }
                    
                }
                else if (userinput == "4")
                {
                    // input pin
                    customer.Pin = getUserPin();
                    goto updateOptions;
                }
                else if (userinput == "5")
                {
                    // update the file
                    data.updateInFile(customer);
                    Console.WriteLine($"\nAccount #{customer.AccountNo} has been successfully been updated.");
                }
                else
                {
                    Console.WriteLine("Invalid input!");
                    goto updateOptions;
                }
            }
            
        }
        else
        {
            Console.WriteLine($"Account Number {accNumber} does not exist.");
        }

    }

    // Search for Account
    public void SearchAccount()
    {
        User user = new User();

        Console.WriteLine("Please provide following information:\n" +
                            "(To leave blank, press enter)\n");
        
        // Get Account number
        string accNumber = string.Empty;
        getAccountNumber:
        {
            Console.Write("Account Number: ");
            accNumber = Console.ReadLine();

            if (!string.IsNullOrEmpty(accNumber))
            {
                try
                {
                    user.AccountNo = Convert.ToInt32(accNumber);
                }
                catch (Exception)
                {
                    Console.WriteLine("\nInvalid Input!\n");
                    goto getAccountNumber;
                }
            }
        }

        // Get user name
        Console.Write("Username: ");
        string uName = Console.ReadLine();
        user.UserName = encryption(uName);

        // Get full name
        Console.Write("First Name: ");
        string fname = Console.ReadLine();
        user.FirstName = fname;

        Console.Write("Last Name: ");
        string lname = Console.ReadLine();
        user.LastName = lname;

        // Get account type
        Console.Write("Account Type (1 = Checking Account, 2 = Saving account): ");
        string accType = getUserSelection(Console.ReadLine(), "Checking", "Saving");
        user.AccountType = accType;

        // Get status
        Console.Write("User Status (1 = Active, 2 = Disabled): ");
        string accStatus = getUserSelection(Console.ReadLine(), "Active", "Disabled");
        user.Status = accStatus;
        
        // Extract info from file
        Data data = new Data();
        List<User> list = data.ReadFile<User>("user.txt");

        List<User> outList = new List<User>();
        if (list.Count > 0)
        {
            foreach (User row in list)
            {
                if (string.IsNullOrEmpty(accNumber))
                {
                    user.AccountNo = row.AccountNo;
                }
                if (string.IsNullOrEmpty(uName))
                {
                    user.UserName = row.UserName;
                }
                if (string.IsNullOrEmpty(fname))
                {
                    user.FirstName = row.FirstName;
                }
                if (string.IsNullOrEmpty(lname))
                {
                    user.LastName = row.LastName;
                }
                if (string.IsNullOrEmpty(accType))
                {
                    user.AccountType = row.AccountType;
                }
                if (string.IsNullOrEmpty(accStatus))
                {
                    user.Status = row.Status;
                }

                if (user.AccountNo == row.AccountNo && 
                user.UserName == row.UserName && 
                user.FirstName == row.FirstName && 
                user.LastName == row.LastName && 
                user.AccountType == row.AccountType && 
                user.Status == row.Status)
                {
                    Console.WriteLine(row.AccountNo);
                    outList.Add(row);
                }
            }

            Console.WriteLine("\n------ Search Results ------\n");
            if (outList.Count>0)
            {
                foreach (User item in outList)
                {
                    Console.WriteLine($"Account No: {item.AccountNo}\n" +
                            $"Username: {encryption(item.UserName)}\n" +
                            $"First Name: {item.FirstName}\n" +
                            $"Last Name: {item.LastName}\n" +
                            $"Account Type: {item.AccountType}\n" +
                            $"Account Status: {item.Status}\n");
                }
            }
            else 
            {
                Console.WriteLine("No Matching Data Found!");
            }
        }
        else
        {
            Console.WriteLine("Sorry, File is Empty!");
        }
    }
    
    // View Report
    public void ViewReports()
    {
        getOption:
        {
            Console.WriteLine("1 - Accounts by Balance\n" +
                              "2 - Acounts by Date");
            
            Console.Write("\nPlease select your option: ");
            string option = Console.ReadLine();
            // By amount
            if (option == "1")
            {
                Console.WriteLine("Please provide start and end amount");
                double min, max;
                getMin:
                {
                    Console.Write("Enter start amount: ");
                    try
                    {
                        min = Convert.ToDouble(Console.ReadLine());
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Invalid Input!");
                        goto getMin;
                    }
                }
                getMax:
                {
                    Console.Write("Enter end amount: ");
                    try
                    {
                        max = Convert.ToDouble(Console.ReadLine());
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Invalid Input!");
                        goto getMax;
                    }
                }

                if (min > max)
                {
                    double temp = max;
                    max = min;
                    min = temp;
                }

                List<User> list = data.ReadFile<User>("user.txt");

                Console.WriteLine("\n------ Search Results ------\n");
                if (list.Count > 0)
                {
                    foreach (User row in list)
                    {
                        if (row.Balance >= min && row.Balance <= max)
                        {
                            Console.WriteLine($"Account No: {row.AccountNo}\n" +
                            $"Username: {encryption(row.UserName)}\n" +
                            $"First Name: {row.FirstName}\n" +
                            $"Last Name: {row.LastName}\n" +
                            $"Account Type: {row.AccountType}\n" +
                            $"Account Status: {row.Status}\n" + 
                            $"Account Balance: {row.Balance}\n");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No Matching Data Found!");
                }
            }
            // By Date
            else if (option == "2")
            {
                Console.WriteLine("Please provide start and end dates");
                string startDate, endDate;
                string formatString = "dd/MM/yyyy";
                DateTime d1, d2;

                getSatrtDate:
                {
                    Console.Write("Enter start date (dd/MM/yyyy): ");
                    startDate = Console.ReadLine();

                    try
                    {
                        d1 = DateTime.ParseExact(startDate, formatString, CultureInfo.InvariantCulture);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Invalid input!");
                        goto getSatrtDate;
                    }
                }

                getEndDate:
                {
                    Console.Write("Enter end date (dd/MM/yyyy): ");
                    endDate = Console.ReadLine();

                    try
                    {
                        d2 = DateTime.ParseExact(endDate, formatString, CultureInfo.InvariantCulture);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Invalid input!");
                        goto getEndDate;
                    }
                }

                if (d1 > d2)
                {
                    (d1, d2) = (d2, d1);
                }

                List<Transaction> transactions = data.ReadFile<Transaction>("transactions.txt");
                Console.WriteLine("\n------ Search Results ------\n");
                if (transactions.Count > 0)
                {   
                    foreach (Transaction row in transactions)
                    {
                        DateTime d = DateTime.ParseExact(row.Date, formatString, CultureInfo.InvariantCulture);

                        if (d >= d1 && d <= d2)
                        {
                            Console.WriteLine($"Transaction Type: {row.TransactionType}\n" +
                            $"Username: {encryption(row.UserName)}\n" +
                            $"First Name: {row.FirstName}\n" +
                            $"Last Name: {row.LastName}\n" +
                            $"Amount: {row.TransactionAmount}\n" +
                            $"Date: {row.Date}\n");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No Matching Data Found!");
                }
            }
            else
            {
                Console.WriteLine("Invalid Input!");
                goto getOption;
            }
        }
    }

    // Cutomer Menu ---------------------------------------------------
    
    // Cash Withdraw
    public void CashWithDraw(string username)
    {
        User user = data.getCustomer(username);

        getOption:
        {
            Console.WriteLine("1 - Fast Cash\n" +
                          "2 - Normal Cash");
            Console.Write("\nPlease select your option: ");

            try
            {
                string option = Console.ReadLine();

                if (option == "1" || option == "2")
                {
                    switch (option)
                    {
                        case "1":
                            Console.Clear();
                            Console.WriteLine("------ Fast Cash ------\n");
                            // list of fast cash options
                            List<double> FastCashOptions = new List<double>(
                                new double[] {20, 60, 100, 140, 160, 200}
                            );

                            Console.WriteLine($"1 - ${FastCashOptions[0]}\n" + 
                                              $"2 - ${FastCashOptions[1]}\n" + 
                                              $"3 - ${FastCashOptions[2]}\n" + 
                                              $"4 - ${FastCashOptions[3]}\n" + 
                                              $"5 - ${FastCashOptions[4]}\n" + 
                                              $"6 - ${FastCashOptions[5]}");

                            getFastCash:
                            {
                                Console.Write("\nPlease select your option: ");
                                string op=Console.ReadLine();

                                if (op=="1" || op=="2" || op=="3" || op=="4" || op=="5" || op=="6")
                                {
                                    int opt = Convert.ToInt32(op);
                                    Console.WriteLine($"You are withdrawing ${FastCashOptions[opt-1]}. Continue (Y/N)?");
                                    if (Console.ReadLine().ToLower().StartsWith("y"))
                                    {
                                        
                                        // total withdrawal cannot exceed 600 per day
                                        double total = PerDayTransactions(user.AccountNo);

                                        if ((total + FastCashOptions[opt - 1]) <= 600)
                                        {
                                            
                                            // has sufficient balance   
                                            if (user != null && user.Balance > FastCashOptions[opt - 1])
                                            {  
                                                // complete the operation
                                                SubstractBalance(user, FastCashOptions[opt-1]);
                                                Console.WriteLine("\nCash Successfully Withdrawn!");

                                                // Commit transaction and update file
                                                Transaction transaction = CommitTransaction(user, FastCashOptions[opt-1], "Cash Withdraw");

                                                // Print receipt
                                                Console.Write("Print a receipt (Y/N)?");
                                                if (Console.ReadLine().ToLower().StartsWith("y"))
                                                {
                                                    PrintReceipt(transaction, "Withdraw");
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("Insufficient Balance. Transaction failed!");
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine($"You have already withdrawn ${total} today.\n" + 
                                                               "You cannot withdraw more then $600 per day.");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Transaction has been canceled!");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid Input!");
                                    goto getFastCash;
                                }
                            }
                            break; // End of Case 1

                        case "2":
                            Console.Clear();
                            Console.WriteLine("------ Normal Cash ------\n");

                            getAmount:
                            {
                                Console.Write("Enter the withdrawal amount in multiples of 20: ");
                                try
                                {
                                    int userInput = Convert.ToInt32(Console.ReadLine());
                                    if (userInput%20==0)
                                    {
                                        Console.WriteLine($"You are withdrawing ${userInput}. Continue (Y/N)?");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid Input!");
                                        goto getAmount;
                                    }

                                    if (Console.ReadLine().ToLower().StartsWith("y")) 
                                    {
                                        double amount = userInput; // converted to monetary type 

                                        // total withdrawal cannot exceed 600 per day
                                        double total = PerDayTransactions(user.AccountNo);

                                        if ((total + amount) <= 600)
                                        {
                                            if (user != null && user.Balance > amount)
                                            {
                                                
                                                SubstractBalance(user, amount);
                                                Console.WriteLine("\nCash Successfully Withdrawn!");

                                                // Commit transaction and update file
                                                Transaction transaction = CommitTransaction(user, amount, "Cash Withdraw");

                                                // Print receipt
                                                Console.Write("Print a receipt (Y/N)?");
                                                if (Console.ReadLine().ToLower().StartsWith("y"))
                                                {
                                                    PrintReceipt(transaction, "Withdraw");
                                                }
                                            }
                                            else 
                                            {
                                                Console.WriteLine("Insufficient Balance. Transaction failed!");
                                            }
                                        
                                        }
                                        else
                                        {
                                            Console.WriteLine($"You have already withdrawn ${total} today.\n" + 
                                                               "You cannot withdraw more then $600 per day.");
                                        }

                                    }
                                    else
                                    {
                                        Console.WriteLine("Transaction has been canceled!");
                                        goto getAmount;
                                    }
                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("Invalid Input!");
                                    goto getAmount;
                                }
                            }
                            break;
                    }
                }
                else // if 1 or 2 is not selected
                {
                    Console.WriteLine("Invalid Input!");
                    goto getOption;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid Input! Exception Error");
                goto getOption;
            }
        }
    }

    // Cash Transfer
    public void CashTransfer(string username)
    {
        User user = new User();
        user = data.getCustomer(username);

        getAmount:
        {
            Console.WriteLine("Enter the amount in multiples of 20: ");

            try
            {
                int userInput = Convert.ToInt32(Console.ReadLine());

                if (userInput%20==0)
                {
                    double amount = userInput; // convert to monetary type
                    if (user.Balance >= amount)
                    {
                        getAccountNo:
                        {
                            Console.WriteLine("Enter the destination account number: ");
                            try
                            {
                                int accNo = Convert.ToInt32(Console.ReadLine());
                                User receiver = new User();
                                if (data.isInFile(accNo, out receiver))
                                {
                                    Console.WriteLine($"You wish to deposite ${amount} in account held by {receiver.FirstName+" "+receiver.LastName}\n" + "Please re-enter the account number to confirm: ");

                                    try
                                    {
                                         int accNo2 = Convert.ToInt32(Console.ReadLine());
                                         if (accNo == accNo2)
                                         {
                                             // substract from sender balance
                                             SubstractBalance(user, amount);

                                             // add to receiver balance
                                             AddToBalance(receiver, amount);

                                             Console.WriteLine("Transaction confirmed!");

                                             // Commit Transaction - Sender account
                                             Transaction transaction = CommitTransaction(user, amount, "Cash Transfer");

                                             // Commit Transaction - Receiver account
                                             Transaction transaction1 = CommitTransaction(receiver, amount, "Cash Transfer");

                                             // Print receipt
                                             Console.Write("Print a receipt (Y/N)?");
                                             if (Console.ReadLine().ToLower().StartsWith("y"))
                                             {
                                                 PrintReceipt(transaction, "Amount Transfered");
                                             }
                                         }
                                         else
                                         {
                                             Console.WriteLine("Did not enter the same account number. Trasaction Failed!");
                                         }

                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine("Did not enter the same account number. Trasaction Failed!");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("The account number does not exist!");
                                }
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Invalid Input!");
                                goto getAccountNo;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Insufficient Balance!!");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Input!");
                } 
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid Input!");
                goto getAmount;
            }
        }
    }

    // Cash Deposite
    public void CashDeposite(string username)
    {
        User user = data.getCustomer(username);

        getAmount:
        {
            Console.WriteLine("Enter the amount: ");

            try
            {
                 int userInput = Convert.ToInt32(Console.ReadLine());
                 double amount = userInput;
                 // add to user balance
                 AddToBalance(user, amount);
                 Console.WriteLine("Cash Deposited Successfully.");

                 // commit transaction
                 Transaction transaction = CommitTransaction(user, amount, "Cash Deposite");

                 // Print receipt
                 Console.Write("Print a receipt (Y/N)?");
                 if (Console.ReadLine().ToLower().StartsWith("y"))
                 {
                     PrintReceipt(transaction, "Amount Deposited");
                 }
            }
            catch (System.Exception)
            { 
                Console.WriteLine("Invalid Input!");
                goto getAmount;
            }
        }

    }
    
    // Display Balance
    public void DisplayBalance(string username)
    {
        User user = data.getCustomer(username);

        Console.WriteLine($"Account Number: {user.AccountNo}");
        DateTime date = DateTime.Now;
        string d = date.ToString("dd/MM/yyyy"); // convert current date time to string
        Console.WriteLine($"Date: {d}");
        Console.WriteLine($"Balance: {user.Balance}");
    }
    
    // Returns total amount withdrawn in a day
    public double PerDayTransactions(int accNo)
    {
        List<Transaction> list = data.ReadFile<Transaction>("transaction.txt");
        double totalAmt = 0;

        foreach (Transaction row in list)
        {
            if (row.AccountNo == accNo)
            {
                totalAmt += row.TransactionAmount;
            }
        }
        return totalAmt;
    }

    // Substract amount from balace
    public void SubstractBalance(User user, double amount)
    {
        user.Balance -= amount;
        data.updateInFile(user);
    }

    // Add amount to balance
    public void AddToBalance(User receiver, double amount)
    {
        receiver.Balance += amount;
        data.updateInFile(receiver);
    }

    // Commit the transaction and update the file
    public Transaction CommitTransaction(User user, double amount, string type)
    {
        Transaction transaction = new Transaction();
        transaction.AccountNo = user.AccountNo;
        transaction.UserName = user.UserName;
        transaction.FirstName = user.FirstName;
        transaction.LastName = user.LastName;
        transaction.TransactionType = type;
        transaction.TransactionAmount = amount;
        DateTime date = DateTime.Now;
        transaction.Date = date.ToString("dd/MM/yyyy");
        transaction.Balance = user.Balance;

        Data data = new Data();
        data.addToFile(transaction);
        return transaction;
    }

    // Print receipt
    public void PrintReceipt(Transaction transaction, string Type)
    {
        Console.WriteLine($"\nAccount Number: {transaction.AccountNo}" + 
                          $"\nDate: {transaction.Date}" + 
                          $"\n{Type}: {transaction.TransactionAmount}" + 
                          $"\nBalance: {transaction.Balance}");
    }
}