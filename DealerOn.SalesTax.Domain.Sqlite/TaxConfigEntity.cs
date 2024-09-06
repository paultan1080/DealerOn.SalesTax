using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using DealerOn.SalesTax.Domain;

// TaxConfig entity
public class TaxConfigEntity
{
    public int Id { get; set; }
    public decimal SalesTaxRate { get; set; } = 0.10m;
    public decimal ImportDutyRate { get; set; } = 0.05m;

    // Exempt categories will be stored as a list of strings
    public List<TaxExemptCategoryEntity> ExemptCategories { get; set; }
}
