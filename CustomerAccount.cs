using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class CustomerAccount
{
    public int Id {get; set;}
    public string AccountName {get; set;}
    public double AccountBalance {get; set;}
    public bool AccountIsActive {get; set;}
    public string AccountCity {get; set;}
    public string AccountPassword {get; set;}

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
}
class CustomerAccountConfiguration : IEntityTypeConfiguration<CustomerAccount>
{
    public void Configure(EntityTypeBuilder<CustomerAccount> builder)
    {
        // Set the primary key
        builder.HasKey(acc => acc.Id);
        builder.HasAlternateKey(acc => acc.AccountName);

        // Set the required properties
        builder.Property(acc => acc.AccountName)
               .IsRequired()
               .HasMaxLength(100); // Set a reasonable max length for the string

        builder.Property(acc => acc.AccountPassword)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(acc => acc.AccountBalance)
               .IsRequired();

        builder.Property(acc => acc.AccountIsActive)
               .IsRequired();

        builder.Property(acc => acc.AccountCity)
               .IsRequired()
               .HasMaxLength(100);

        // Configure table name
        builder.ToTable("CustomerAccounts");
    }
}