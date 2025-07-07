using OpenQA.Selenium;

namespace ClientApplicationTestProject.Pages
{
    public class CartPage : BasePage
    {
        public CartPage(IWebDriver driver) : base(driver) { }

        private By CheckoutBtn => By.CssSelector("li.totalRow .btn");
        private By cartProductNames => By.CssSelector(".cartSection h3");
        private By DeleteBtns => By.ClassName("btn-danger");
        private By ContinueShoppingBtn => By.CssSelector(".btn-primary[routerlink*='dashboard']");
        private By NoProductsText => By.XPath("//div[@class='ng-star-inserted']/h1");

        /// Gets a read-only collection of all product name elements currently displayed in the cart.  
        /// Uses the specified selector to find product name elements within the cart section.  
        public IReadOnlyCollection<IWebElement> CartItems => WaitForElementsVisible(cartProductNames);


        public bool IsAtCartPage()
        {
            if (WaitForElementClickable(ContinueShoppingBtn).Displayed)
            {
                return true;
            }
            else
                return false;
        }
        /// Checks if a product with the specified name exists in the cart.  
        /// Performs a case-insensitive comparison against all product names currently listed in the cart.  
        public string ProductsInCart()
        {
            foreach (var item in CartItems)
            {
                return item.Text;
            }
            return string.Empty; // Return an empty string if no items are found in the cart  
        }

        public bool ProductsAvailableInCart()
        {
            var deleteButtons = WaitForElementsVisible(DeleteBtns);
            if (deleteButtons != null && deleteButtons.Any()) // Check if the collection is not null and contains elements  
            {
                return true;
            }
            else
            {
                Console.WriteLine(WaitGetElementText(NoProductsText));
                return false; // Return false when no products are found  
            }
        }

        public OrderReviewPage ProceedToCheckout()
        {
            try
            {
                var deleteButtons = WaitForElementsVisible(DeleteBtns);
                if (deleteButtons != null && deleteButtons.Any())
                {
                    WaitForElementClickable(CheckoutBtn);
                    WaitAndClick(CheckoutBtn);
                    return new OrderReviewPage(Driver);
                }
                else
                {
                    Console.WriteLine("No products available in the cart to proceed to checkout.");
                    return null; // Return null if no products are available in the cart
                }
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine($"Element not found while proceeding to checkout: {ex.Message}");
                throw new InvalidOperationException("Failed to proceed to checkout - element not found", ex);
            }

        }

        public void DeleteCartProduct()
        {
            var deleteButtons = WaitForElementsVisible(DeleteBtns);
            foreach (var btn in deleteButtons)
            {
                btn.Click();
            }
        }

        public DashboardPage ContinueShopping()
        {
            WaitAndClick(ContinueShoppingBtn);
            return new DashboardPage(Driver);
        }
    }
}
