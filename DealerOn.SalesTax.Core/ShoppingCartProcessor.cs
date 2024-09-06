using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DealerOn.SalesTax.Domain;

namespace DealerOn.SalesTax.Core;

public class ShoppingCartProcessor
{
    private readonly List<Func<RawItem, ItemTrait?>> _classifiers;
    private readonly TaxConfig _taxConfig;

    public ShoppingCartProcessor(List<Func<RawItem, ItemTrait?>> classifiers, TaxConfig taxConfig = null)
    {
        _classifiers = classifiers;
        _taxConfig = taxConfig ?? new TaxConfig(); // Use default tax config if none provided
    }

    public ShoppingCartProcessor(IShoppingCartRepository repository)
    {
        // Load classifier definitions from the repository
        var classifierDefinitions = repository.GetClassifierDefinitions();
        _classifiers = ClassifierFactory.CreateClassifiers(classifierDefinitions);

        // Load tax config from the repository
        _taxConfig = repository.GetTaxConfig();
    }

    // Processes the cart and applies traits
    public ShoppingCart ProcessCart(RawShoppingCart<RawItem> rawCart)
    {
        var processedItems = rawCart.Items
            .Select(item => ProcessItem(item))
            .ToArray();

        return new ShoppingCart(processedItems, rawCart.Quantities);
    }

    // Applies tax rules based on item traits
    public void ApplyTaxRules(ShoppingCart cart)
    {
        foreach (var item in cart.Items)
        {
            decimal salesTax = 0;
            decimal importDuty = 0;

            // Check if the item is exempt from sales tax based on its category
            var categoryTrait = item.Traits.OfType<ItemCategory>().FirstOrDefault();
            if (categoryTrait == null || !_taxConfig.SalesTaxExemptCategories.Contains(categoryTrait.CategoryName))
            {
                salesTax = item.Price * _taxConfig.SalesTaxRate;
            }

            // Check if the item is imported
            var isImported = item.Traits.OfType<ItemIsImported>().Any();
            if (isImported)
            {
                importDuty = item.Price * _taxConfig.ImportDutyRate;
            }

            // Calculate total taxes
            var totalTax = salesTax + importDuty;
            var totalPriceWithTax = item.Price + totalTax;

            // Output result
            Console.WriteLine($"{item.Name}: {totalPriceWithTax:C} (Sales Tax: {salesTax:C}, Import Duty: {importDuty:C})");
        }
    }

    // Applies each classifier to an item to build up the list of traits
    private Item ProcessItem(RawItem rawItem)
    {
        var traits = _classifiers
            .Select(classifier => classifier(rawItem))  // Apply each function to the item
            .Where(trait => trait != null)              // Filter out non-matching traits (nulls)
            .ToArray();

        return new Item(rawItem.Name, rawItem.Price, traits);
    }
}
