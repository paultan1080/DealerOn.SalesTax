using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DealerOn.SalesTax.Domain;

namespace DealerOn.SalesTax.Core;

public class ReceiptGenerator
{
    private readonly TaxConfig _taxConfig;

    public ReceiptGenerator(TaxConfig taxConfig)
    {
        _taxConfig = taxConfig;
    }

    // Generates a receipt from the shopping cart
    public string GenerateReceipt(ShoppingCart cart)
    {
        var receiptLines = new List<string>();
        decimal totalSalesTax = 0;
        decimal totalAmount = 0;

        // Consolidate items by name and price
        var consolidatedItems = ConsolidateItems(cart);

        // Process each item, calculate taxes and total prices
        foreach (var (item, quantity) in consolidatedItems)
        {
            var (itemTotalPrice, itemSalesTax, taxPerItem) = CalculateItemTotals(item, quantity);
            totalSalesTax += itemSalesTax;
            totalAmount += itemTotalPrice;

            // Format the receipt line for the item
            string itemLine = FormatItemLine(item, quantity, itemTotalPrice, taxPerItem);
            receiptLines.Add(itemLine);
        }

        if(totalSalesTax > 0) 
        {
            // Add sales taxes and total to the receipt
            receiptLines.Add($"Sales Taxes: {totalSalesTax:F2}");
        }
        receiptLines.Add($"Total: {totalAmount:F2}");

        return string.Join(Environment.NewLine, receiptLines);
    }

    // Consolidates items based on name and price
    private Dictionary<Item, int> ConsolidateItems(ShoppingCart cart)
    {
        return cart.Items
            .GroupBy(item => new { item.Name, item.Price })
            .ToDictionary(g => g.First(), g => g.Count());
    }

    // Calculates total price and sales tax for an item
    private (decimal totalPrice, decimal salesTax, decimal taxPerItem) CalculateItemTotals(Item item, int quantity)
    {
        decimal salesTax = 0;
        decimal importDuty = 0;

        // Check if item is taxable based on its category
        var categoryTrait = item.Traits.OfType<ItemCategory>().FirstOrDefault();
        if (categoryTrait == null || !_taxConfig.SalesTaxExemptCategories.Contains(categoryTrait.CategoryName))
        {
            salesTax = item.Price * _taxConfig.SalesTaxRate;
        }

        // Check if item is imported
        var isImported = item.Traits.OfType<ItemIsImported>().Any();
        if (isImported)
        {
            importDuty = item.Price * _taxConfig.ImportDutyRate;
        }

        // Total tax per item
        var totalTaxPerItem = salesTax + importDuty;

        // Check if total cents value is divisible by 5; if not, round up
        if((totalTaxPerItem * 100) % 5 != 0) {
            totalTaxPerItem = Math.Ceiling(totalTaxPerItem * 20) / 20;
        }

        // Calculate total price with taxes applied
        var totalItemPrice = (item.Price + totalTaxPerItem) * quantity;

        return (totalItemPrice, totalTaxPerItem * quantity, totalTaxPerItem);
    }

    // Formats the line for an individual item, including quantity if > 1
    private string FormatItemLine(Item item, int quantity, decimal totalPrice, decimal itemSalesTax)
    {
        string quantityStr = quantity > 1 ? $"({quantity} @ {(item.Price + itemSalesTax):F2})" : "";
        return $"{item.Name}: {totalPrice:F2} {quantityStr}".Trim();
    }
}
