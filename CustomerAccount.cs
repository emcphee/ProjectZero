using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

class CustomerAccount
{

    #region Properties
    [Key]
    public int AccountNo {get; set;}
    public string AccountName {get; set;}
    public double AccountBalance {get; set;}
    public bool AccountIsActive {get; set;}
    public string AccountCity {get; set;}
    public string AccountPassword {get; set;}
    #endregion

    #region Methods
    public CustomerAccount(string accountName, double accountBalance, bool accountIsActive, string accountCity, string accountPassword)
    {
        AccountName = accountName;
        AccountBalance = accountBalance;
        AccountIsActive = accountIsActive;
        AccountCity = accountCity;
        AccountPassword = accountPassword;
    }
    
    public bool Withdraw(double amount)
    {
        if (AccountBalance < amount)
        {
            return false;
        }
        AccountBalance -= amount;
        return true;
    }

    public void Deposit(double amount)
    {
        AccountBalance += amount;
    }
    #endregion

}