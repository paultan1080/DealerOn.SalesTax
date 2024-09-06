using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DealerOn.SalesTax.Domain;

namespace DealerOn.SalesTax.Core;

public static class ClassifierFactory
{
    // Creates a list of classifier functions from classifier definitions
    public static List<Func<RawItem, ItemTrait?>> CreateClassifiers(List<ClassifierDefinition> definitions)
    {
        return definitions.Select(CreateClassifier).ToList();
    }

    // Creates a single classifier function based on the definition type
    public static Func<RawItem, ItemTrait?> CreateClassifier(ClassifierDefinition definition)
    {
        return definition.Type switch
        {
            "Keyword" => (Func<RawItem, ItemTrait?>)(item =>
            {
                if (item.Name.Contains(definition.Pattern, StringComparison.OrdinalIgnoreCase))
                {
                    return CreateTrait(definition.TraitType, definition.TraitValue);
                }
                return null;
            }),
            "ExactMatch" => (Func<RawItem, ItemTrait?>)(item =>
            {
                if (item.Name.Equals(definition.Pattern, StringComparison.OrdinalIgnoreCase))
                {
                    return CreateTrait(definition.TraitType, definition.TraitValue);
                }
                return null;
            }),
            "Regex" => (Func<RawItem, ItemTrait?>)(item =>
            {
                var regex = new Regex(definition.Pattern, RegexOptions.IgnoreCase);
                if (regex.IsMatch(item.Name))
                {
                    return CreateTrait(definition.TraitType, definition.TraitValue);
                }
                return null;
            }),
            "Wildcard" => (Func<RawItem, ItemTrait?>)(_ =>
            {
                return CreateTrait(definition.TraitType, definition.TraitValue); // Matches everything
            }),
            _ => throw new InvalidOperationException($"Unsupported classifier type: {definition.Type}")
        };
    }

    // Creates the appropriate ItemTrait based on the trait type and value
    private static ItemTrait CreateTrait(string traitType, string traitValue)
    {
        return traitType switch
        {
            "ItemIsImported" => new ItemIsImported(),
            "ItemCategory" => new ItemCategory(traitValue),
            _ => throw new InvalidOperationException($"Unsupported trait type: {traitType}")
        };
    }
}