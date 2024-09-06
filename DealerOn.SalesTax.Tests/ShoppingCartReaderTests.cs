using Xunit;
using System.Collections.Generic;
using System.Linq;

using DealerOn.SalesTax.Core;
using DealerOn.SalesTax.Domain;

public class ShoppingCartReaderTests
{
    [Fact]
    public void ShoppingCartReader_ShouldParseSingleItemCorrectly()
    {
        // Arrange
        var input = new List<string> { "1 Book at 12.49" };

        // Act
        var cart = ShoppingCartReader.CreateShoppingCartFromInput(input);

        // Assert
        Assert.Single(cart.Items);
        Assert.Equal("Book", cart.Items[0].Name);
        Assert.Equal(12.49m, cart.Items[0].Price);
        Assert.Single(cart.Quantities);
        Assert.Equal(1, cart.Quantities[0]);
    }

    [Fact]
    public void ShoppingCartReader_ShouldParseMultipleItemsCorrectly()
    {
        // Arrange
        var input = new List<string>
        {
            "1 Book at 12.49",
            "2 Music CDs at 14.99"
        };

        // Act
        var cart = ShoppingCartReader.CreateShoppingCartFromInput(input);

        // Assert
        Assert.Equal(2, cart.Items.Length);
        Assert.Equal("Book", cart.Items[0].Name);
        Assert.Equal(12.49m, cart.Items[0].Price);
        Assert.Equal("Music CDs", cart.Items[1].Name);
        Assert.Equal(14.99m, cart.Items[1].Price);

        Assert.Equal(1, cart.Quantities[0]);
        Assert.Equal(2, cart.Quantities[1]);
    }

    [Fact]
    public void ShoppingCartReader_ShouldThrowExceptionOnInvalidInput()
    {
        // Arrange
        var input = new List<string> { "Invalid input" };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => ShoppingCartReader.CreateShoppingCartFromInput(input));
    }
}

