using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace ClientApplicationTestProject.Pages
{
    public class DashboardPage : BasePage
    {

        private By SignOutButton => By.XPath("//button[text()=' Sign Out ']");
        private By HomeButton => By.XPath("//button[text()=' HOME ']");
        private By ProductList => By.CssSelector(".card-body"); // card of each product
        private By CartIconLink => By.CssSelector("button[routerlink*='cart']");
        private By SuccessMessage => By.CssSelector(".toast-success");
        private By AddToCartSuccessMessage => By.Id("#toast-container");
        private By CartItemsNumber => By.CssSelector("button[routerlink*='/dashboard/cart'] label");
        private By AddToCartBtns => By.ClassName("fa-shopping-cart");

        public DashboardPage(IWebDriver driver) : base(driver) { }

        public string ProductAddedText() => WaitForElementVisible(SuccessMessage).Text;

        // Fix for CS0149: Method name expected  
        // The issue is caused by the incorrect instantiation of the `Actions` class.  
        // The correct syntax is to use the constructor of `Actions` with the `IWebDriver` instance.  

        public string AddProductToCartByIndex(int index)
        {
            var products = WaitForElementsVisible(ProductList).ToList();

            // Check if the product list is empty  
            if (products.Count == 0)
            {
                // Take screenshot for debugging  
                TakeScreenshot("NoProductsAvailable");
                throw new NoSuchElementException("No products are available on the dashboard page.");
            }

            if (products.Count <= index)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of Range for the product list");
            }
            var productName = products[index].FindElement(By.CssSelector("b")).Text;
            var addToCartButton = products[index].FindElements(AddToCartBtns).FirstOrDefault();

            // Correct instantiation of Actions  
            Actions action = new Actions(Driver);
            action.MoveToElement(addToCartButton).Click().Perform();

            waitForLoadingToDisappear();
            return productName;
        }

        public bool AddProductToCartByName(string productName)
        {
            var products = WaitForElementsVisible(ProductList).ToList();

            foreach (var product in products)
            {
                string productTitle = product.FindElement(By.CssSelector("b")).Text;

                if (productTitle.Equals(productName, StringComparison.OrdinalIgnoreCase))
                {
                    // Find the add to cart button within this product
                    var addToCartButton = product.FindElement(AddToCartBtns);

                    // Wait for element to be clickable before attempting to click
                    Wait.Until(driver => addToCartButton.Displayed && addToCartButton.Enabled);

                    addToCartButton.Click();
                    // Correct instantiation of Actions  
                   // Actions action = new Actions(Driver);
                   // action.MoveToElement(addToCartButton).Click().Perform();

                  //  var element = WaitForElementVisible(CartItemsNumber);
                    //var text = element.Text;
                   // Wait.Until(driver => element.Displayed && int.TryParse(text, out int value) && value > 0);
                    return true; // Exit the method immediately after performing the action
                }
            }
            // Product not found, throw exception
            throw new ArgumentException($"Product with name '{productName}' was not found on the dashboard.", nameof(productName));
            return false;
        }

        public bool SignOutVisible() => WaitForElementVisible(SignOutButton).Displayed;
        public string GetAddedToCartToastedMessage()
        {
            return WaitForToastAndGetText(AddToCartSuccessMessage);
        }

        //returning multiple elements from Method using Tuples 
        public (bool homeDisplayed, bool signoutDisplayed) HasLoggedIn()
        {
            return (WaitForElementVisible(HomeButton).Displayed, WaitForElementVisible(SignOutButton).Displayed);
        }
        public bool waitForLoadingToDisappear()
        {
            WaitUntilInvisible(AddToCartSuccessMessage);
            return true;
        }

        public int GetCartItemValue()
        {
            WaitForElementVisible(CartItemsNumber);
            try
            {
                Wait.Until(driver =>
                {
                    try
                    {
                        var element = WaitForElementVisible(CartItemsNumber);
                        var text = element.Text;
                        return !string.IsNullOrWhiteSpace(text) && int.TryParse(text, out int value) && value > 0;
                    }
                    catch (StaleElementReferenceException)
                    {
                        return false;
                    }
                });
                var cartValue = int.Parse(WaitForElementVisible(CartItemsNumber).Text);
                return cartValue;
            }
            catch (WebDriverTimeoutException) 
            {
                TakeScreenshot("CartItemValye_Timeout");
                return 0;
            }
        }
        public CartPage GoToCart()
        {
            waitForLoadingToDisappear();
            WaitForElementClickable(CartIconLink);
            WaitAndClick(CartIconLink);
            var cartPage = new CartPage(Driver);
            try
            {
                if (!cartPage.IsAtCartPage())
                {
                    throw new InvalidOperationException("Navigation to Cart Page failed: Cart page is not displayed.");
                }
            }
            catch (Exception ex)
            {
                TakeScreenshot("GoToCart_Failed");
                throw new Exception("Exception occurred while verifying Cart Page navigation.", ex);
            }
            return cartPage;
        }

    }
}
