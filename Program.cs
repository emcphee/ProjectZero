using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
internal class Program
{

    private static void Main(string[] args) // Menu to navigate to Customer or Admin Account
    {
        BankDBContext bankDBContext = new BankDBContext();

        bool DEBUG = false;
        if(DEBUG)
        {
            var t = bankDBContext.Transactions.First();
            Console.WriteLine(t.sourceAccount.AccountName);

            // Create admin account
            //bankDBContext.AdminAccounts.Add(new AdminAccount("admin", "admin"));
            //var customeracc = bankDBContext.CustomerAccounts.FirstOrDefault(a => a.AccountName == "user");
            //bankDBContext.CustomerAccounts.Add(customeracc);
            //bankDBContext.Tickets.Add(new CustomerTicket(customeracc.Id, "Password"));
            //bankDBContext.SaveChanges();
            return;
        }


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
        CustomerAccount? account = AttemptLoginCustomer(bankDBContext);
        if(account ==  null){
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
                    CustomerCheckAccountDetails(bankDBContext, account);
                    break;
                case "2": // Withdraw
                    CustomerWithdraw(bankDBContext, account);
                    break;
                case "3": // Deposit
                    CustomerDeposit(bankDBContext, account);
                    break;
                case "4": // Transfer
                    CustomerTransfer(bankDBContext, account);
                    break;
                case "5": // Last 5 transactions
                    CustomerLast5Transactions(bankDBContext, account);
                    break;
                case "6": // Request Cheque Book
                    CustomerRequestChequeBook(bankDBContext, account);
                    break;
                case "7": // Change Password
                    CustomerChangePassword(bankDBContext, account);
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
        AdminAccount? adminAcc = AttemptLoginAdmin(bankDBContext);
        if(adminAcc == null)
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
                    ShowAccountSummary(bankDBContext);
                    break;
                case "5": // Reset Customer Password
                    ResetPassword(bankDBContext);
                    break;
                case "6": // Approve Cheque book request
                    ApproveChequeRequest(bankDBContext, adminAcc.Id);
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

    private static CustomerAccount? AttemptLoginCustomer(BankDBContext bankDBContext) // checks database to see if password is correct
    {
        Console.Write("Enter Username: ");
        string? usernameInput = Console.ReadLine();

        Console.Write("Enter Password: ");
        string? passwordInput = Console.ReadLine();
        Console.Clear(); // instantly clear so password is not on the screen

        CustomerAccount? account = bankDBContext.CustomerAccounts.FirstOrDefault(x => x.AccountName.Equals(usernameInput));
        if(account == null || account.AccountPassword != passwordInput)
        {
            Console.WriteLine("Invalid credetials.");
            return null;
        }
        return account;
    }
    private static AdminAccount? AttemptLoginAdmin(BankDBContext bankDBContext) // checks database to see if password is correct
    {
        Console.Write("Enter Username: ");
        string? usernameInput = Console.ReadLine();

        Console.Write("Enter Password: ");
        string? passwordInput = Console.ReadLine();
        Console.Clear(); // instantly clear so password is not on the screen

        AdminAccount? account = bankDBContext.AdminAccounts.FirstOrDefault(x => x.AccountName == usernameInput);
        if(account == null || account.AccountPassword != passwordInput)
        {
            Console.WriteLine("Invalid credentials.");
            return null;
        } 
        Console.WriteLine("Welcome back!");
        return account;
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
            Console.Write("Enter username of account to delete: ");
            string accountName = Console.ReadLine();
            acc = bankDBContext.CustomerAccounts.FirstOrDefault(x => x.AccountName.Equals(accountName));
        }
        catch
        {
            Console.WriteLine("Failed to parse account name.");
            Console.ReadKey();
            return false;
        }
        if(acc == null)
        {
            Console.WriteLine("Account does not exist.");
            Console.ReadKey();
            return false;
        }
        bankDBContext.CustomerAccounts.Remove(acc);
        Console.WriteLine("Account deleted.");
        Console.ReadKey();
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
            Console.ReadKey();
            return false;
        }

        acc = bankDBContext.CustomerAccounts.FirstOrDefault(x => x.AccountName.Equals(accountName));
        if(acc == null)
        {
            Console.WriteLine("Account does not Exist.");
            Console.ReadKey();
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
            Console.ReadKey();
            return false;
        }
        field = field.ToLower();

        if (field == null) 
        {
            Console.WriteLine("Bad new value inputted.");
            Console.ReadKey();
            return false;
        }
        string? newVal;

        Console.Write("\nNew Value:");
        switch(field)
        {            
            case "name":
            newVal = Console.ReadLine();
            if(newVal == null)
            {
                Console.WriteLine("New Field Not Entered.");
                Console.ReadKey();
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
                Console.ReadKey();
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
                Console.ReadKey();
                return false;
            }
            break;

            case "city":
            newVal = Console.ReadLine();
            if(newVal == null)
            {
                Console.WriteLine("New Field Not Entered.");
                Console.ReadKey();
                return false;
            }
            acc.AccountCity = newVal;
            break;
            
            case "password":
            newVal = Console.ReadLine();
            if(newVal == null)
            {
                Console.WriteLine("New Field Not Entered.");
                Console.ReadKey();
                return false;
            }
            acc.AccountPassword = newVal;
            break;
            
            default:
            Console.WriteLine("Invalid field inputted.");
            Console.ReadKey();
            return false;
        }
        
        // save changes made to db
        bankDBContext.CustomerAccounts.Update(acc);
        bankDBContext.SaveChanges();
        Console.WriteLine("Change Made.");
        Console.ReadKey();
        return true;
    }
    private static bool ResetPassword(BankDBContext bankDBContext)
    {
        Console.Write("Enter username to change password for: ");
        string? user = Console.ReadLine();
        CustomerAccount? acc = bankDBContext.CustomerAccounts.FirstOrDefault(x => x.AccountName == user);
        if(acc == null)
        {
            Console.WriteLine("User not found.");
            Console.ReadKey();
            return false;
        }
        Console.Write("Enter new password: ");
        string? pass1 = Console.ReadLine();
        Console.Write("Re-enter password: ");
        string? pass2 = Console.ReadLine();
        if(pass1 == null || pass1 != pass2)
        {
            Console.WriteLine("Invalid input.");
            Console.ReadKey();
            return false;
        }
        acc.AccountPassword = pass1;
        bankDBContext.CustomerAccounts.Update(acc);
        bankDBContext.SaveChanges();
        Console.WriteLine("Password updated successfully.");
        Console.ReadKey();
        return true;
    }
    private static bool ApproveChequeRequest(BankDBContext bankDBContext, int adminId)
    {
        List<CustomerTicket> unprocessedTickets = bankDBContext.Tickets
                                                .Where(t => t.TicketType == "ChequeBookRequest" && t.ResponderAccountID == null)
                                                .ToList();
        try
        {
            Console.WriteLine("Select which Cheque Request to accept/deny");
            for(int i = 1; i <= unprocessedTickets.Count; i++)
            {
                CustomerTicket ticket = unprocessedTickets[i - 1];
                Console.WriteLine($"{i}. Requester: {ticket.SenderAccount.AccountName}");
            }
            int input = Convert.ToInt32(Console.ReadLine()) - 1;
            CustomerTicket curTicket = unprocessedTickets[input];
            Console.WriteLine("Would you like to accept or deny this request? (accept/deny):");
            string? input2 = Console.ReadLine().ToLower();
            if(input2 == "accept")
            {
                curTicket.ResponderAccountID = adminId;
                // here is where emailing and actually sending them a chequebook would happen
            }
            else if(input2 == "deny")
            {
                curTicket.ResponderAccountID = adminId;
                // here is where emailing them the denial would happen
            }
            else
            {
                Console.WriteLine("Invalid input.");
                Console.ReadKey();
                return false;
            }

            bankDBContext.Tickets.Update(curTicket);
            bankDBContext.SaveChanges();
        }
        catch
        {
            Console.WriteLine("Error in selecting a Cheque Request.");
            Console.ReadKey();
            return false;
        }
        Console.WriteLine("Ticket completed.");
        Console.ReadKey();
        return true;
    }
    private static void ShowAccountSummary(BankDBContext bankDBContext)
    {
        Console.Write("Enter username to show summary for: ");

        string? user = Console.ReadLine();
        CustomerAccount? acc = bankDBContext.CustomerAccounts.FirstOrDefault(x => x.AccountName == user);
        if(acc == null)
        {
            Console.WriteLine("User not found.");
            return;
        }
        Console.WriteLine($"Username: {acc.AccountName}\nBalance: {acc.AccountBalance}\nAccountIsActive: {acc.AccountIsActive}\nAccountCity: {acc.AccountCity}");
        Console.ReadKey();
    }
    private static void CustomerCheckAccountDetails(BankDBContext bankDBContext, CustomerAccount account)
    {
        string details = $"Balance: {account.AccountBalance}\n"
                       + $"Account Username: {account.AccountName}\n"
                       + $"City: {account.AccountCity}\n"
                       + $"IsActive: {account.AccountIsActive}";
        Console.WriteLine(details);
        Console.ReadKey();
    }
    private static bool CustomerWithdraw(BankDBContext bankDBContext, CustomerAccount account)
    {
        double amt;
        Console.Write("How much would you like to withdraw?: ");
        try
        {
            amt = Convert.ToDouble(Console.ReadLine());
        }
        catch
        {
            Console.WriteLine("Invalid Amount.");
            Console.ReadKey();
            return false;
        }
        bool success = account.Withdraw(amt);
        if(success)
        {
            Console.WriteLine("Withdraw success!");
            bankDBContext.CustomerAccounts.Update(account);
            bankDBContext.SaveChanges();
            Console.ReadKey();
            return true;
        }
        else // failure
        {
            Console.WriteLine("Not enough funds.");
            Console.ReadKey();
            return false;
        }
    }
    private static void CustomerDeposit(BankDBContext bankDBContext, CustomerAccount account)
    {
        double amt;
        Console.Write("How much would you like to deposit?: ");
        try
        {
            amt = Convert.ToDouble(Console.ReadLine());
        }
        catch
        {
            Console.WriteLine("Invalid Amount.");
            Console.ReadKey();
            return;
        }
        account.Deposit(amt);
        bankDBContext.CustomerAccounts.Update(account);
        bankDBContext.SaveChanges();
        Console.WriteLine("Deposit success!");
        Console.ReadKey();
    }
    private static bool CustomerTransfer(BankDBContext bankDBContext, CustomerAccount account)
    {
        Console.Write("Who would you like to send money?: ");
        string? recipientUsername = Console.ReadLine();
        Console.Write("How much would you like to send?: ");
        string? amtStr = Console.ReadLine();
        try
        {
            if(recipientUsername == null) return false;
            CustomerAccount? recipAcc = bankDBContext.CustomerAccounts.FirstOrDefault(account => account.AccountName == recipientUsername);
            if(recipAcc == null)
            {
                Console.WriteLine("Couldn't find Recipient Account.");
                Console.ReadKey();
                return false;
            }
            double amt = Convert.ToDouble(amtStr);
            if(amt > account.AccountBalance)
            {
                Console.WriteLine("Not enough funds.");
                Console.ReadKey();
                return false;
            }
            account.AccountBalance -= amt;
            recipAcc.AccountBalance += amt;
            bankDBContext.CustomerAccounts.Update(recipAcc);
            bankDBContext.CustomerAccounts.Update(account);

            Transaction transaction = new Transaction(account.Id, recipAcc.Id, amt);
            bankDBContext.Transactions.Add(transaction);
            bankDBContext.SaveChanges();
            Console.WriteLine("Funds transferred.");
            Console.ReadKey();
            return true;
        }
        catch
        {
            Console.WriteLine("Error in account or amount input.");
            Console.ReadKey();
        }
        return false;
    }
    private static void CustomerLast5Transactions(BankDBContext bankDBContext, CustomerAccount account)
    {
        // select 5 Transactions where account.Id == Transaction.SenderId, sort by datetime
        var recentTransactions = bankDBContext.Transactions
        .Where(t => t.sourceAccountId == account.Id || t.destinationAccountId == account.Id)
        .OrderByDescending(t => t.UnixTimestamp)
        .Take(5)
        .ToList();
        // print up to 5 transactions
        Console.WriteLine("-- 5 Recent Transactions --");
        foreach(var transaction in recentTransactions)
        {
            Console.WriteLine($"{transaction.sourceAccount.AccountName} sent {transaction.destinationAccount.AccountName} {transaction.Amount}");
        }
        Console.ReadKey();
    }
    private static void CustomerRequestChequeBook(BankDBContext bankDBContext, CustomerAccount account)
    {
        // create CustomerTicket with ChequeBook request
        CustomerTicket ticket = new CustomerTicket(account.Id, "ChequeBookRequest");
        bankDBContext.Tickets.Add(ticket);
        bankDBContext.SaveChanges();
        Console.WriteLine("Request Completed.");
        Console.ReadKey();
    }
    private static void CustomerChangePassword(BankDBContext bankDBContext, CustomerAccount account)
    {
        // prompt for new password and change if valid
        Console.Write("Enter new password: ");
        string? firstEnter = Console.ReadLine();
        Console.Write("Re-Enter new password: ");
        string? secondEnter = Console.ReadLine();
        if (firstEnter == null || firstEnter != secondEnter)
        {
            Console.WriteLine("Passwords don't match");
            return;
        }
        account.AccountPassword = firstEnter;
        bankDBContext.CustomerAccounts.Update(account);
        bankDBContext.SaveChanges();
        Console.WriteLine("Password updated.");
        Console.ReadKey();
    }
}