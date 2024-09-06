namespace DealerOn.SalesTax.Domain;

// ClassifierDefinition model to represent rules that can be stored in a database
public class ClassifierDefinition
{
    public string Type { get; set; } // "Keyword", "ExactMatch", "Regex", "Wildcard"
    public string Pattern { get; set; } // The keyword, regex pattern, or exact match string
    public string TraitType { get; set; } // The type of trait ("ItemIsImported" or "ItemCategory")
    public string TraitValue { get; set; } // The value associated with the trait (e.g., category name for ItemCategory)
}