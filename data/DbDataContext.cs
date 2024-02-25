using Microsoft.EntityFrameworkCore;
using heystock.models;

namespace heystock.data;

public class DbDataContext : DbContext
{
    public DbDataContext(DbContextOptions<DbDataContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Produto> Produtos { get; set; } = null!;
    public DbSet<Venda> Vendas { get; set; } = null!;
    public DbSet<Stock> Stocks {get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /* modelBuilder.Entity<User>()
            .HasMany(e => e.chats)
            .WithOne(e => e.user)
            .HasForeignKey(e => e.user_id)
            .HasPrincipalKey(e => e.id); */

        modelBuilder.Entity<Chat>()
            .HasMany(e => e.chatPrompts)
            .WithOne(e => e.chat)
            .HasForeignKey(e => e.conversation_id)
            .HasPrincipalKey(e => e.id);
        
        modelBuilder.Entity<Chat>()
            .HasMany(e => e.userPrompts)
            .WithOne(e => e.chat)
            .HasForeignKey(e => e.conversation_id)
            .HasPrincipalKey(e => e.id);

        modelBuilder.Entity<UserPrompt>()
            .Property(u => u.prompt_number)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<ChatSucessfullResponse>()
            .Property(csr => csr.response_number)
            .ValueGeneratedOnAdd();

        base.OnModelCreating(modelBuilder); 
    }
}