using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using DealerOn.SalesTax.Domain;

public class ShoppingCartRepository : IShoppingCartRepository
{
    private readonly SalesTaxDbContext _dbContext;

    public ShoppingCartRepository(SalesTaxDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<ClassifierDefinition> GetClassifierDefinitions()
    {
        return _dbContext.ClassifierDefinitions
            .Select(cd => new ClassifierDefinition
            {
                Type = cd.Type,
                Pattern = cd.Pattern,
                TraitType = cd.TraitType,
                TraitValue = cd.TraitValue
            }).ToList();
    }

    public TaxConfig GetTaxConfig()
    {
        var taxConfigEntity = _dbContext.TaxConfigs
            .Include(c => c.ExemptCategories)
            .FirstOrDefault();

        var taxConfig = new TaxConfig
        {
            SalesTaxRate = taxConfigEntity.SalesTaxRate,
            ImportDutyRate = taxConfigEntity.ImportDutyRate,
            SalesTaxExemptCategories = taxConfigEntity.ExemptCategories
                .Select(ec => ec.CategoryName)
                .ToHashSet()
        };

        return taxConfig;
    }
}
