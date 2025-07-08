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
        private ThankYouOrderPage _thankYouOrderPage;
        
       
        public TestDataCleaner(IWebDriver driver)
        {
            _driver = driver;
        }

        public void DeleteTestOrder()
        {
            string orderId = TestDataCache.Get<string>("LastCreatedOrderId"); // Specify the type argument explicitly
            TestDataCache.Remove("LastCreatedOrderId");

            _orderPage = new OrdersPage(_driver);
            _orderPage.DeleteOrder(orderId);
        }

        public void DeletePlacedOrder() 
        {
            _thankYouOrderPage = new ThankYouOrderPage(_driver);
            string orderId = _thankYouOrderPage.ExtractOrderId_FromURL();
            _thankYouOrderPage.GoToOrderHistoryPage();
            _orderPage = new OrdersPage(_driver);
            _orderPage.DeleteOrder(orderId);
        }

        public void ClearCart()
        {
            _cartPage = new CartPage(_driver);
            _cartPage.DeleteCartProduct();
        }

        public void GoToCartPageIfItemsExistAndClearCart() 
        {
            _mainMenuPage = new MainMenuPage(_driver);
            bool productsInCart = _mainMenuPage.IsItemsExistsInCart();
            if (productsInCart == false) 
            {
                _mainMenuPage.SignOut();
            }
            else
            {
                _mainMenuPage.GoToCart();
                ClearCart();
                _mainMenuPage.SignOut();
            }    
        }

        public void SignOut()
        {
            _mainMenuPage = new MainMenuPage(_driver);
            _mainMenuPage.SignOut();
        }
    }
}
