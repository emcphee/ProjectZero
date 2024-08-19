using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
public class AdminAccount
{


    public int Id { get; set; }
    public string AccountName { get; set; }
    public string AccountPassword { get; set; }

    public AdminAccount(string AccountName, string AccountPassword)
    {
        this.AccountName = AccountName;
        this.AccountPassword = AccountPassword;
    }
}

class AdminAccountConfiguration : IEntityTypeConfiguration<AdminAccount>
{
    public void Configure(EntityTypeBuilder<AdminAccount> builder)
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

        // Configure table name
        builder.ToTable("AdminAccounts");
    }
}