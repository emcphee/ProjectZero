using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
public class CustomerTicket
{

    public int Id {get; set;}
    public string TicketType {get; set;}
    
    // Account of admin which fulfils the ticket. Default null.
    public int? ResponderAccountID {get; set;}
    public virtual AdminAccount? ResponserAccount {get; set;}

    // Account of user which makes the Ticket.
    public int SenderAccountID {get; set;}
    public virtual CustomerAccount? SenderAccount {get; set;}


    public CustomerTicket(int SenderAccountID, string TicketType)
    {
        this.SenderAccountID = SenderAccountID;
        this.TicketType = TicketType;
        ResponderAccountID = null;
    }
}
class CustomerTicketConfiguration : IEntityTypeConfiguration<CustomerTicket>
{
    public void Configure(EntityTypeBuilder<CustomerTicket> builder)
    {
        // Set the primary key
        builder.HasKey(ct => ct.Id);

        // Set the required properties
        builder.Property(ct => ct.TicketType)
               .IsRequired()
               .HasMaxLength(50); // Set a reasonable max length for the string

        builder.Property(ct => ct.SenderAccountID)
               .IsRequired();

        // Configure relationships

        // SenderAccount is required
        builder.HasOne(ct => ct.SenderAccount)
               .WithMany()
               .HasForeignKey(ct => ct.SenderAccountID)
               .OnDelete(DeleteBehavior.Restrict); // To prevent cascade delete

        // ResponderAccount is optional
        builder.HasOne(ct => ct.ResponserAccount)
               .WithMany()
               .HasForeignKey(ct => ct.ResponderAccountID)
               .OnDelete(DeleteBehavior.SetNull); // Setting null on delete since it's nullable

        // Configure table name
        builder.ToTable("CustomerTickets");
    }
}