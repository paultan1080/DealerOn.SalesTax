namespace DealerOn.SalesTax.Domain;

// Base RawItem record
public record RawItem(string Name, decimal Price);

// Item inherits from RawItem and has parsed traits
public record Item(string Name, decimal Price, ItemTrait[] Traits) : RawItem(Name, Price);

// Item traits and categories
public abstract record ItemTrait;

// Trait signifying that the item is imported
public sealed record ItemIsImported() : ItemTrait;

// Item category
public record ItemCategory(string CategoryName) : ItemTrait;