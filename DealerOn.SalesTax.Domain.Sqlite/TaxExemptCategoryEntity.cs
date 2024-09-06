using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using DealerOn.SalesTax.Domain;

// TaxExemptCategory entity (one-to-many with TaxConfig)
public class TaxExemptCategoryEntity
{
    public int Id { get; set; }
    public string CategoryName { get; set; }
    public int TaxConfigEntityId { get; set; }
}
