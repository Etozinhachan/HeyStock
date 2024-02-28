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
    public DbSet<Distribuidor> distribuidores { get; set; } = null!;
    public DbSet<Fornecedor> fornecedores { get; set; } = null!;
    public DbSet<Marca> marcas { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(e => e.vendas)
            .WithOne(e => e.user)
            .HasForeignKey(e => e.userId)
            .HasPrincipalKey(e => e.id); 

        modelBuilder.Entity<Venda>()
            .HasMany(e => e.produtos)
            .WithOne(e => e.venda)
            .HasForeignKey(e => e.vendaId)
            .HasPrincipalKey(e => e.id);
        
        modelBuilder.Entity<Stock>()
            .HasMany(e => e.produtos)
            .WithOne(e => e.stock)
            .HasForeignKey(e => e.stockId)
            .HasPrincipalKey(e => e.id);

        /* modelBuilder.Entity<UserPrompt>()
            .Property(u => u.prompt_number)
            .ValueGeneratedOnAdd(); 
        
        modelBuilder.Entity<ChatSucessfullResponse>()
            .Property(csr => csr.response_number)
            .ValueGeneratedOnAdd(); */

        modelBuilder.Entity<Distribuidor>()
            .HasMany(e => e.produtos)
            .WithOne(e => e.distribuidor)
            .HasForeignKey(e => e.distribuidorId)
            .HasPrincipalKey(e => e.id);

        modelBuilder.Entity<Fornecedor>()
            .HasMany(e => e.stocks)
            .WithOne(e => e.fornecedor)
            .HasForeignKey(e => e.fornecedorId)
            .HasPrincipalKey(e => e.id);
        
        modelBuilder.Entity<Marca>()
            .HasMany(e => e.produtos)
            .WithOne(e => e.marca)
            .HasForeignKey(e => e.marcaId)
            .HasPrincipalKey(e => e.id);

        modelBuilder.Entity<Marca>()
            .HasMany(e => e.fornecedores)
            .WithOne(e => e.marca)
            .HasForeignKey(e => e.marcaId)
            .HasPrincipalKey(e => e.id);

        modelBuilder.Entity<Marca>()
            .HasMany(e => e.distribuidores)
            .WithOne(e => e.marca)
            .HasForeignKey(e => e.MarcaId)
            .HasPrincipalKey(e => e.id);

        base.OnModelCreating(modelBuilder); 
    }
}