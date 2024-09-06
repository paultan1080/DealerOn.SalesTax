namespace DealerOn.SalesTax.Domain;

public class TaxConfig
{
    public decimal SalesTaxRate { get; set; } = 0.10m; // 10% by default
    public decimal ImportDutyRate { get; set; } = 0.05m; // 5% by default
    public HashSet<string> SalesTaxExemptCategories { get; set; } = new HashSet<string> { "Food", "Medical", "Books" }; // Default exempt categories
}
