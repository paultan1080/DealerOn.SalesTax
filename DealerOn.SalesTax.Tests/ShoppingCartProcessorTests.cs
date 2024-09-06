using Xunit;
using System.Collections.Generic;
using System.Linq;

using DealerOn.SalesTax.Core;
using DealerOn.SalesTax.Domain;

namespace DealerOn.SalesTax.Tests;

public class ShoppingCartProcessorTests
{
    [Fact]
    public void KeywordClassifier_ShouldAddTrait_WhenItemMatchesKeyword()
    {
        // Arrange
        var classifierDefinition = new ClassifierDefinition
        {
            Type = "Keyword",
            Pattern = "imported",
            TraitType = "ItemIsImported",
            TraitValue = ""
        };
        var classifier = ClassifierFactory.CreateClassifier(classifierDefinition);

        var rawItem = new RawItem("imported chocolate", 5.99m);

        // Act
        var trait = classifier(rawItem);

        // Assert
        Assert.IsType<ItemIsImported>(trait);
    }

    [Fact]
    public void ExactMatchClassifier_ShouldAddTrait_WhenItemExactlyMatches()
    {
        // Arrange
        var classifierDefinition = new ClassifierDefinition
        {
            Type = "ExactMatch",
            Pattern = "book",
            TraitType = "ItemCategory",
            TraitValue = "Books"
        };
        var classifier = ClassifierFactory.CreateClassifier(classifierDefinition);

        var rawItem = new RawItem("book", 12.49m);

        // Act
        var trait = classifier(rawItem);

        // Assert
        var categoryTrait = Assert.IsType<ItemCategory>(trait);
        Assert.Equal("Books", categoryTrait.CategoryName);
    }

    [Fact]
    public void RegexClassifier_ShouldAddTrait_WhenItemMatchesPattern()
    {
        // Arrange
        var classifierDefinition = new ClassifierDefinition
        {
            Type = "Regex",
            Pattern = "book",
            TraitType = "ItemCategory",
            TraitValue = "Books"
        };
        var classifier = ClassifierFactory.CreateClassifier(classifierDefinition);

        var rawItem = new RawItem("novel book", 9.99m);

        // Act
        var trait = classifier(rawItem);

        // Assert
        var categoryTrait = Assert.IsType<ItemCategory>(trait);
        Assert.Equal("Books", categoryTrait.CategoryName);
    }

    [Fact]
    public void WildcardClassifier_ShouldAddTrait_ToAllItems()
    {
        // Arrange
        var classifierDefinition = new ClassifierDefinition
        {
            Type = "Wildcard",
            TraitType = "ItemCategory",
            TraitValue = "Unclassified"
        };
        var classifier = ClassifierFactory.CreateClassifier(classifierDefinition);

        var rawItem = new RawItem("some random item", 1.99m);

        // Act
        var trait = classifier(rawItem);

        // Assert
        var categoryTrait = Assert.IsType<ItemCategory>(trait);
        Assert.Equal("Unclassified", categoryTrait.CategoryName);
    }

    [Fact]
    public void ShoppingCartProcessor_ShouldProcessItems_AndApplyTraits()
    {
        // Arrange
        var classifierDefinitions = new List<ClassifierDefinition>
        {
            new ClassifierDefinition { Type = "Keyword", Pattern = "imported", TraitType = "ItemIsImported", TraitValue = "" },
            new ClassifierDefinition { Type = "ExactMatch", Pattern = "book", TraitType = "ItemCategory", TraitValue = "Books" },
            new ClassifierDefinition { Type = "Wildcard", TraitType = "ItemCategory", TraitValue = "Unclassified" }
        };
        var classifiers = ClassifierFactory.CreateClassifiers(classifierDefinitions);

        var processor = new ShoppingCartProcessor(classifiers);

        var rawItem1 = new RawItem("imported chocolate", 5.99m);
        var rawItem2 = new RawItem("book", 12.49m);
        var rawCart = new RawShoppingCart<RawItem>(new[] { rawItem1, rawItem2 }, new[] { 1, 2 });

        // Act
        var completedCart = processor.ProcessCart(rawCart);

        // Assert
        var item1 = completedCart.Items[0];
        Assert.Contains(item1.Traits, t => t is ItemIsImported);
        Assert.Contains(item1.Traits, t => t is ItemCategory && ((ItemCategory)t).CategoryName == "Unclassified");

        var item2 = completedCart.Items[1];
        Assert.Contains(item2.Traits, t => t is ItemCategory && ((ItemCategory)t).CategoryName == "Books");
        Assert.Contains(item2.Traits, t => t is ItemCategory && ((ItemCategory)t).CategoryName == "Unclassified");
    }
}
