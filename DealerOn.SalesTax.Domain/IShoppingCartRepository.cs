namespace DealerOn.SalesTax.Domain;

public interface IShoppingCartRepository
{
    List<ClassifierDefinition> GetClassifierDefinitions();
    TaxConfig GetTaxConfig();
}