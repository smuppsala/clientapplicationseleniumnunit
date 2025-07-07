using ClientApplicationTestProject.Pages;
using OpenQA.Selenium;

namespace ClientApplicationTestProject.Pages
{
    public class MainMenuPage : BasePage
    {
        public MainMenuPage(IWebDriver driver) : base(driver) { }

        private By SignOutButton => By.XPath("//button[text()=' Sign Out ']");
        private By HomeButton => By.XPath("//button[text()=' HOME ']");
        private By CartIcon => By.CssSelector("button[routerlink*='cart']");
        private By CartItemsNumber => By.XPath("//button[@class='btn btn-custom']//label");

        public void SignOut()
        {
            WaitAndClick(SignOutButton);
        }

        public CartPage GoToCartWhenHaveItemsInIt()
        {
            var cartValue = GetNumberOfProductsInCart();
            if (cartValue != null)
            {
                WaitForElementVisible(CartItemsNumber);
                WaitAndClick(CartIcon);
                return new CartPage(Driver);
            }
            return null;
        }

        public string GetNumberOfProductsInCart()
        {
            var cartValue = WaitGetElementText(CartItemsNumber);
            return cartValue;
        }

        public bool IsItemsExistsInCart()
        {
            var cartValue = GetNumberOfProductsInCart();
            if (String.IsNullOrEmpty(cartValue))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
