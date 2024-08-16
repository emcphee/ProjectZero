internal class Program
{

    private static void Main(string[] args) // Menu to navigate to Customer or Admin Account
    {
        BankDBContext bankDBContext = new BankDBContext();
        string? userInput;
        bool exit = false;
        while(!exit)
        {

            Console.WriteLine("What account would you like to access?");
            Console.WriteLine("1. Customer");
            Console.WriteLine("2. Admin");
            Console.WriteLine("3. Exit");
            userInput = Console.ReadLine();
            switch(userInput)
            {
                case "1": // customer
                    CustomerMain(bankDBContext);
                    break;
                case "2": // admin
                    AdminMain(bankDBContext);
                    break;
                case "3": // exit
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid input!");
                    break;
            }
        }
        Console.Clear();
        Console.WriteLine("Thank you for banking with us!");
    }
    private static void CustomerMain(BankDBContext bankDBContext) // Menu for customers
    {
        bool loginSuccess = AttemptLogin("Customer", bankDBContext);
        if(!loginSuccess)
        {
            Console.WriteLine("Login failed.");
            return;
        }

        bool exit = false;
        while(!exit)
        {
            Console.WriteLine("1. Check Account Details");
            Console.WriteLine("2. Withdraw");
            Console.WriteLine("3. Deposit");
            Console.WriteLine("4. Transfer");
            Console.WriteLine("5. Last 5 transactions");
            Console.WriteLine("6. Request Cheque Book");
            Console.WriteLine("7. Change Password");
            Console.WriteLine("8. Exit");
            string? userInput = Console.ReadLine();

            switch(userInput)
            {
                case "1": // Check Account Details
                    
                    break;
                case "2": // Withdraw
                    break;
                case "3": // Deposit
                    break;
                case "4": // Transfer
                    break;
                case "5": // Last 5 transactions
                    break;
                case "6": // Request Cheque Book
                    break;
                case "7": // Change Password
                    break;
                case "8": // Exit
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid input.");
                    break;
            }
            Console.Clear();
        }
        Console.WriteLine("Goodbye!");
    }

    private static void AdminMain(BankDBContext bankDBContext) // Admin Control Panel
    {
        bool loginSuccess = AttemptLogin("Admin", bankDBContext);
        if(!loginSuccess)
        {
            Console.WriteLine("Login failed.");
            return;
        }

        bool exit = false;
        while(!exit)
        {
            Console.WriteLine("1. Create New Account");
            Console.WriteLine("2. Delete Account");
            Console.WriteLine("3. Edit Account Details");
            Console.WriteLine("4. Display Summary ");
            Console.WriteLine("5. Reset Customer Password");
            Console.WriteLine("6. Approve Cheque book request");
            Console.WriteLine("7. Exit");
            string? userInput = Console.ReadLine();

            switch(userInput)
            {
                case "1": // Create New Account
                    CustomerAccount? newAccount = CreateNewCustomerAccount();
                    if(newAccount == null) break;
                    bankDBContext.CustomerAccounts.Add(newAccount);
                    bankDBContext.SaveChanges();
                    Console.WriteLine("New Account Created!");
                    break;
                case "2": // Delete Account
                    bool deleteSuccess = DeleteCustomerAccount(bankDBContext);
                    if(deleteSuccess)
                    {
                        Console.WriteLine("Account successfully deleted.");
                    }
                    else
                    {
                        Console.WriteLine("Error: Account deletion failed, likely didn't exist.");
                    }
                    break;
                case "3": // Edit Account Details
                    EditCustomerAccount(bankDBContext);
                    break;
                case "4": // Display Summary 

                    break;
                case "5": // Reset Customer Password

                    break;
                case "6": // Approve Cheque book request

                    break;
                case "7": // Exit
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid input.");
                    break;
            }
            Console.Clear();
        }
        Console.WriteLine("Goodbye!");
    }

    private static bool AttemptLogin(string accountType, BankDBContext bankDBContext) // checks database to see if password is correct
    {
        Console.WriteLine($"You are logging in with account type: {accountType}");

        Console.Write("Enter Username: ");
        string? usernameInput = Console.ReadLine();

        Console.Write("Enter Password: ");
        string? passwordInput = Console.ReadLine();
        Console.Clear(); // instantly clear so password is not on the screen

        if(accountType == "Admin")
        {
            AdminAccount? account = bankDBContext.AdminAccounts.FirstOrDefault(x => x.AccountName.Equals(usernameInput));
            if(account == null || account.AccountPassword != passwordInput)
            {
                Console.WriteLine("Invalid credentials.");
                return false;
            } 
            Console.WriteLine("Welcome back!");
            return true;
        }
        if(accountType == "Customer")
        {
            CustomerAccount? account = bankDBContext.CustomerAccounts.FirstOrDefault(x => x.AccountName.Equals(usernameInput));
            if(account == null || account.AccountPassword != passwordInput)
            {
                Console.WriteLine("Invalid credetials.");
                return false;
            }
            return true;
        }
        return false;
    }

    private static CustomerAccount? CreateNewCustomerAccount()
    {
        CustomerAccount? customerAccount = null;
        try
        {
            Console.Write("Enter Customer AccountName: ");
            string accountName = Console.ReadLine();
            Console.Write("Enter Initial Deposit: ");
            double initialBalance = Convert.ToDouble(Console.ReadLine());
            Console.Write("Enter Customer City: ");
            string accountCity = Console.ReadLine();
            Console.Write("Enter Customer Password: ");
            string accountPassword = Console.ReadLine();
            customerAccount = new CustomerAccount(accountName, initialBalance, true, accountCity, accountPassword);
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error setting up new account. MSG:{ex.Message}\n");
            return null;
        }

        return customerAccount;
    }

    /* Console UI prompting and database commands to delete a customer account. Returns success */
    private static bool DeleteCustomerAccount(BankDBContext bankDBContext)
    {
        CustomerAccount? acc;
        try
        {
            string accountName = Console.ReadLine();
            acc = bankDBContext.CustomerAccounts.FirstOrDefault(x => x.AccountName.Equals(accountName));
        }
        catch
        {
            Console.WriteLine("Failed to parse account name.");
            return false;
        }
        if(acc == null)
        {
            Console.WriteLine("Account does not exist.");
            return false;
        }
        bankDBContext.CustomerAccounts.Remove(acc);
        return true;
    }
    /* Prompts admin for an account, then a field to edit. Returns success*/
    private static bool EditCustomerAccount(BankDBContext bankDBContext)
    {
        CustomerAccount? acc;
        Console.Write("Input Account Name to Edit: ");
        string? accountName = Console.ReadLine();
        if(accountName == null)
        {
            Console.WriteLine("No Account Name Inputted.");
            return false;
        }

        acc = bankDBContext.CustomerAccounts.FirstOrDefault(x => x.AccountName.Equals(accountName));
        if(acc == null)
        {
            Console.WriteLine("Account does not Exist.");
            return false;
        }

        Console.WriteLine("Enter field to change from the following");
        Console.WriteLine("- Balance");
        Console.WriteLine("- Name");
        Console.WriteLine("- City");
        Console.WriteLine("- IsActive");
        Console.WriteLine("- Password");
        string? field = Console.ReadLine();
        if (field == null) 
        {
            Console.WriteLine("Bad field inputted.");
            return false;
        }
        field = field.ToLower();
        string? newValue = Console.ReadLine();
        if (field == null) 
        {
            Console.WriteLine("Bad new value inputted.");
            return false;
        }
        
        string? newVal;
        switch(field)
        {            
            case "name":
            newVal = Console.ReadLine();
            if(newVal == null)
            {
                Console.WriteLine("New Field Not Entered.");
                return false;
            }
            acc.AccountName = newVal;
            break;

            case "balance":
            try
            {
                double newBalance = Convert.ToDouble(Console.ReadLine());
                acc.AccountBalance = newBalance;
            }
            catch
            {
                Console.WriteLine("Invalid New Balance.");
                return false;
            }
            break;

            case "isactive":
            try
            {
                bool newActiveState;
                newActiveState = Convert.ToBoolean(Console.ReadLine());
                acc.AccountIsActive = newActiveState;
            }
            catch
            {
                Console.WriteLine("Invalid New Account Activity State.");
                return false;
            }
            break;

            case "city":
            newVal = Console.ReadLine();
            if(newVal == null)
            {
                Console.WriteLine("New Field Not Entered.");
                return false;
            }
            acc.AccountCity = newVal;
            break;
            
            case "password":
            newVal = Console.ReadLine();
            if(newVal == null)
            {
                Console.WriteLine("New Field Not Entered.");
                return false;
            }
            acc.AccountPassword = newVal;
            break;
            
            default:
            Console.WriteLine("Invalid field inputted.");
            return false;
        }
        Console.WriteLine("Change Made.");
        
        // save changes made to db
        bankDBContext.CustomerAccounts.Update(acc);
        bankDBContext.SaveChanges();
        return true;
    }
    private static bool ResetPassword(BankDBContext bankDBContext)
    {
        return true;
    }
    private static bool ApproveChequeRequest(BankDBContext bankDBContext)
    {
        return true;
    }
    private static void ShowAccountSummary(BankDBContext bankDBContext)
    {

    }
    Console.WriteLine("1. Check Account Details");
    Console.WriteLine("2. Withdraw");
    Console.WriteLine("3. Deposit");
    Console.WriteLine("4. Transfer");
    Console.WriteLine("5. Last 5 transactions");
    Console.WriteLine("6. Request Cheque Book");
    Console.WriteLine("7. Change Password");
    Console.WriteLine("8. Exit");
    private static void CustomerCheckAccountDetails(BankDBContext bankDBContext, CustomerAccount account)
    {

    }
    private static bool CustomerWithdraw(BankDBContext bankDBContext, CustomerAccount account)
    {
        return true;
    }
    private static void CustomerDeposit(BankDBContext bankDBContext, CustomerAccount account)
    {
        
    }
    private static bool CustomerTransfer(BankDBContext bankDBContext, CustomerAccount account)
    {
        return true;
    }
    private static void CustomerLast5Transactions(BankDBContext bankDBContext, CustomerAccount account)
    {

    }
    private static void CustomerRequestChequeBook(BankDBContext bankDBContext, CustomerAccount account)
    {

    }
    private static void CustomerChangePassword(BankDBContext bankDBContext, CustomerAccount account)
    {

    }
    private static Tuple<>

} 
    