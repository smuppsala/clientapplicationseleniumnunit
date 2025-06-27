using OpenQA.Selenium;

namespace ClientApplicationTestProject.Pages
{
    public class CartPage : BasePage
    {
       // private readonly IWebDriver Driver;

        public CartPage(IWebDriver driver) : base(driver) { }


        //  private IWebElement CheckoutBtn => Driver.FindElement(By.CssSelector("li.totalRow button"));
        private By CheckoutBtn => By.CssSelector("li.totalRow button");
        private By cartProductNames => By.CssSelector(".cartSection h3");
        private By DeleteBtns => By.ClassName("btn-danger");

        /// Gets a read-only collection of all product name elements currently displayed in the cart.
        /// Uses the specified selector to find product name elements within the cart section.
        private IReadOnlyCollection<IWebElement> CartItems =>
                Driver.FindElements(cartProductNames);

        /// Checks if a product with the specified name exists in the cart.
        /// Performs a case-insensitive comparison against all product names currently listed in the cart.
        public bool IsProductInCart(string productName)
        {
            return CartItems.Any(item => item.Text.Equals(productName, System.StringComparison.OrdinalIgnoreCase));
        }

        public void ProceedToCheckout()
        {
            WaitAndClick(CheckoutBtn);
        }

        public void DeleteCartProduct() 
        {

                var deleteButtons = WaitForElementsVisible(DeleteBtns);
                foreach (var btn in deleteButtons)
                {
                    btn.Click();
                }
        }
    }
}
