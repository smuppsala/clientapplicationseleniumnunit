using ClientApplication.Pages;
using ClientApplicationTestProject.Pages;
using OpenQA.Selenium;

namespace ClientApplicationTestProject.Utilities
{
    public class TestDataCleaner
    {
        private readonly IWebDriver _driver;
        private OrdersPage _orderPage;
        private CartPage _cartPage;
        private DashboardPage _dashboardPage;
        private MainMenuPage _mainMenuPage;
        public TestDataCleaner(IWebDriver driver)
        {
            _driver = driver;
        }

        public void DeleteTestOrder(string orderId)
        {
            _orderPage = new OrdersPage(_driver);
            _orderPage.DeleteOrder(orderId);
        }

        public void ClearCart()
        {
            _cartPage = new CartPage(_driver);
            _cartPage.DeleteCartProduct();
        }

        public void GoToCartPageIfItemsExist() 
        {
            _dashboardPage = new DashboardPage(_driver);
            var cartItems = _dashboardPage.GetNumberOfProductsInCart();
            if (cartItems != null) 
            {
                _dashboardPage.GoToCart();
            }
        }

        public void SignOut()
        {
            _mainMenuPage = new MainMenuPage(_driver);
            _mainMenuPage.SignOut();
        }
    }
}
