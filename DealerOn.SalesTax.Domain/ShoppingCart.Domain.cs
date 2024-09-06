namespace DealerOn.SalesTax.Domain;

// RawShoppingCart<T> record with a generic type for items
public record RawShoppingCart<TItem>(TItem[] Items, int[] Quantities);

// ShoppingCart is the parsed/ingested version of RawShoppingCart
public record ShoppingCart(Item[] Items, int[] Quantities) : RawShoppingCart<Item>(Items, Quantities);
