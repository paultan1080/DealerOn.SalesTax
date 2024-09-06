using DealerOn.SalesTax.Core;
using DealerOn.SalesTax.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DealerOn.SalesTax.Web.Pages
{
    public class ShoppingBasketProcessorModel : PageModel
    {
        private readonly IShoppingCartRepository _repository;

        public ShoppingBasketProcessorModel(IShoppingCartRepository repository)
        {
            _repository = repository;
        }

        [BindProperty]
        public string ShoppingCartInput { get; set; }

        public string Receipt { get; private set; }

        public void OnGet()
        {
            // Display empty form on first load
        }

        public void OnPost()
        {
            if (!string.IsNullOrWhiteSpace(ShoppingCartInput))
            {
                // Parse the input from the text box using the ShoppingCartReader
                var inputLines = ShoppingCartInput.Split(Environment.NewLine);
                var rawCart = ShoppingCartReader.CreateShoppingCartFromInput(inputLines);

                // Get the classifiers and tax config from the database
                var classifierDefinitions = _repository.GetClassifierDefinitions();
                var classifiers = ClassifierFactory.CreateClassifiers(classifierDefinitions); // Convert to functions
                var taxConfig = _repository.GetTaxConfig();

                // Create a processor to process the cart
                var processor = new ShoppingCartProcessor(classifiers, taxConfig);
                var completedCart = processor.ProcessCart(rawCart);

                // Generate the receipt
                var receiptGenerator = new ReceiptGenerator(taxConfig);
                Receipt = receiptGenerator.GenerateReceipt(completedCart);
            }
        }
    }
}