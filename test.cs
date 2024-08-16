internal class Test
{
    private static void notMain(string[] args)
    {
        BankDBContext bankDBContext = new BankDBContext();
    
        AdminAccount? acc = bankDBContext.AdminAccounts.Find(1);

        System.Console.WriteLine(acc.AccountName);
        bankDBContext.SaveChanges();


        Console.WriteLine("testing");
    }
}