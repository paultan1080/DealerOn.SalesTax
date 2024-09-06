using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DealerOn.SalesTax.Domain;

namespace DealerOn.SalesTax.Core;

public class ShoppingCartReader
{
    // Parses input lines and creates a RawShoppingCart<RawItem>
    public static RawShoppingCart<RawItem> CreateShoppingCartFromInput(IEnumerable<string> inputLines)
    {
        var items = new List<RawItem>();
        var quantities = new List<int>();

        foreach (var input in inputLines)
        {
            // Parse the line "<quantity> <name> at <price>"
            var parts = input.Split(" at ");
            if (parts.Length != 2)
            {
                throw new ArgumentException($"Invalid input format: '{input}'. Expected '<quantity> <name> at <price>'");
            }

            var quantityPart = parts[0].Split(" ", 2);
            if (!int.TryParse(quantityPart[0], out int quantity))
            {
                throw new ArgumentException($"Invalid quantity in input: '{quantityPart[0]}'");
            }

            var itemName = quantityPart[1];
            if (!decimal.TryParse(parts[1], out decimal price))
            {
                throw new ArgumentException($"Invalid price in input: '{parts[1]}'");
            }

            // Add the item to the cart
            items.Add(new RawItem(itemName, price));
            quantities.Add(quantity);
        }

        return new RawShoppingCart<RawItem>(items.ToArray(), quantities.ToArray());
    }
}

