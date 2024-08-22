using Microsoft.EntityFrameworkCore;

        
class BankDBContext : DbContext
{
    public DbSet<AdminAccount> AdminAccounts { get; set; }
    public DbSet<CustomerAccount> CustomerAccounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<CustomerTicket> Tickets { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=DESKTOP-EN9FMHH\\SQLEXPRESS;Database=BANK_DB;User Id=user;Password=password;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new CustomerTicketConfiguration());
        builder.ApplyConfiguration(new AdminAccountConfiguration());
        builder.ApplyConfiguration(new CustomerAccountConfiguration());
        builder.ApplyConfiguration(new TransactionConfiguration());
    }
}