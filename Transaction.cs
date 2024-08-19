using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class Transaction
{

    public int Id {get; set;}
    public long UnixTimestamp {get; set;}
    public double Amount {get; set;}
    
    public int sourceAccountId {get; set;}
    public virtual CustomerAccount sourceAccount {get; set;}

    public int destinationAccountId {get; set;}
    public virtual CustomerAccount destinationAccount {get; set;}
    public Transaction(int sourceAccountId, int destinationAccountId, double amount)
    {
        UnixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        this.sourceAccountId= sourceAccountId;
        this.destinationAccountId = destinationAccountId;
        this.Amount = amount;
    }
}

class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        // Set the primary key
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Amount)
               .IsRequired();
        
        builder.Property(t => t.UnixTimestamp)
               .IsRequired();

        // Configure relationships

        builder.HasOne(t => t.sourceAccount)
               .WithMany()
               .HasForeignKey(t => t.sourceAccountId)
               .OnDelete(DeleteBehavior.Restrict); // To prevent cascade delete
        
        builder.HasOne(t => t.destinationAccount)
               .WithMany()
               .HasForeignKey(t => t.destinationAccountId)
               .OnDelete(DeleteBehavior.Restrict); // To prevent cascade delete

        // Configure table name
        builder.ToTable("Transactions");
    }
}