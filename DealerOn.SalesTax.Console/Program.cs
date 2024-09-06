using System;
using DealerOn.SalesTax.Core;
using DealerOn.SalesTax.Domain;

class Program
{
    static void Main(string[] args)
    {
        // Step 1: Setup rules and tax config
        var (classifiers, taxConfig) = Setup();

        List<string> inputLines;

        if (Console.IsInputRedirected)
        {
            // If input is being piped, read all lines at once
            inputLines = ReadPipedInput();
        }
        else
        {
            // Interactive mode: show the prompt and read from stdin
            Console.WriteLine("Enter items (format: '<quantity> <name> at <price>'), one per line. Enter an empty line to finish:");
            inputLines = ReadInputLinesFromStdin();
        }

        // Step 2: Create shopping cart using the factory
        var rawCart = ShoppingCartReader.CreateShoppingCartFromInput(inputLines);

        // Step 3: Process the shopping cart
        var processor = new ShoppingCartProcessor(classifiers, taxConfig);
        var completedCart = processor.ProcessCart(rawCart);

        // Step 4: Generate and print the receipt
        var receiptGenerator = new ReceiptGenerator(taxConfig);
        var receipt = receiptGenerator.GenerateReceipt(completedCart);

        Console.WriteLine("\nReceipt:");
        Console.WriteLine(receipt);
    }

    // Setup classifiers and tax config
    static (List<Func<RawItem, ItemTrait?>>, TaxConfig) Setup()
    {
        var classifierDefinitions = new List<ClassifierDefinition>
        {
            new ClassifierDefinition { Type = "Keyword", Pattern = "imported", TraitType = "ItemIsImported", TraitValue = "" },
            new ClassifierDefinition { Type = "Keyword", Pattern = "book", TraitType = "ItemCategory", TraitValue = "Books" },
            new ClassifierDefinition { Type = "Keyword", Pattern = "chocolate", TraitType = "ItemCategory", TraitValue = "Food" },
            new ClassifierDefinition { Type = "Keyword", Pattern = "pills", TraitType = "ItemCategory", TraitValue = "Medical" }
        };

        var classifiers = ClassifierFactory.CreateClassifiers(classifierDefinitions);
        var taxConfig = new TaxConfig
        {
            SalesTaxRate = 0.10m,
            ImportDutyRate = 0.05m,
            SalesTaxExemptCategories = new HashSet<string> { "Books", "Food", "Medical" }
        };

        return (classifiers, taxConfig);
    }

    // Reads input from stdin until an empty line is entered (interactive mode)
    static List<string> ReadInputLinesFromStdin()
    {
        var inputLines = new List<string>();
        while (true)
        {
            var input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
                break;
            inputLines.Add(input);
        }
        return inputLines;
    }

    // Reads input when piped (batch mode)
    static List<string> ReadPipedInput()
    {
        var inputLines = new List<string>();
        string input;
        while ((input = Console.ReadLine()) != null)
        {
            inputLines.Add(input);
        }
        return inputLines;
    }
}
