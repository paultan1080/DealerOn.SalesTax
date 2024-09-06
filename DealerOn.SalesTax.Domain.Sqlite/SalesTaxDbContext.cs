using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using DealerOn.SalesTax.Domain;

public class SalesTaxDbContext : DbContext
{
    public DbSet<ClassifierDefinitionEntity> ClassifierDefinitions { get; set; }
    public DbSet<TaxConfigEntity> TaxConfigs { get; set; }
    public DbSet<TaxExemptCategoryEntity> TaxExemptCategories { get; set; }

    public SalesTaxDbContext()
        : base()
    { }

    public SalesTaxDbContext(DbContextOptions<SalesTaxDbContext> options)
        : base(options)
    { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=../sales_tax.db");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed initial classifier definitions
        modelBuilder.Entity<ClassifierDefinitionEntity>().HasData(
            new ClassifierDefinitionEntity { Id = 1, Type = "Keyword", Pattern = "imported", TraitType = "ItemIsImported", TraitValue = "" },
            new ClassifierDefinitionEntity { Id = 2, Type = "Keyword", Pattern = "book", TraitType = "ItemCategory", TraitValue = "Books" },
            new ClassifierDefinitionEntity { Id = 3, Type = "Keyword", Pattern = "chocolate", TraitType = "ItemCategory", TraitValue = "Food" },
            new ClassifierDefinitionEntity { Id = 4, Type = "Keyword", Pattern = "pills", TraitType = "ItemCategory", TraitValue = "Medical" }
        );

        // Seed initial tax config
        modelBuilder.Entity<TaxConfigEntity>().HasData(
            new TaxConfigEntity { Id = 1, SalesTaxRate = 0.10m, ImportDutyRate = 0.05m }
        );

        // Seed exempt categories
        modelBuilder.Entity<TaxExemptCategoryEntity>().HasData(
            new TaxExemptCategoryEntity { Id = 1, CategoryName = "Food", TaxConfigEntityId = 1 },
            new TaxExemptCategoryEntity { Id = 2, CategoryName = "Books", TaxConfigEntityId = 1 },
            new TaxExemptCategoryEntity { Id = 3, CategoryName = "Medical", TaxConfigEntityId = 1 }
        );
    }
}