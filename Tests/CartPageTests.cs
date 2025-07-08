using AventStack.ExtentReports;
using ClientApplicationTestProject.Pages;

namespace ClientApplicationTestProject.Tests
{
    public class CartPageTests : TestBase
    {
        private LoginPage _loginPage;
        private DashboardPage _dashboardPage;
        private MainMenuPage _mainmenuPage;
        private CartPage _cartPage;
        private OrderReviewPage _orderReviewPage;
        private readonly string productName = "ZARA COAT 3";

        [SetUp]
        public void SetUp()
        {
            _loginPage = new LoginPage(Driver);
            _loginPage.GoTo();
            _dashboardPage = _loginPage.LoginWithSecrets();
            _cartPage = _dashboardPage.GoToCart();
        }

        [Test]
        public void AddedProductDisplayed_InCart()
        {
            bool productsAvaliability = _cartPage.ProductsAvailableInCart();
            Assert.That(productsAvaliability, Is.False, "There's No products available in Your Cart");
            Assert.That(_cartPage.CartItems.Count, Is.Zero, "No Products in Your Cart !");

            _dashboardPage = _cartPage.ContinueShopping();
            string productName = _dashboardPage.AddProductToCartByIndex(1);
            _cartPage = _dashboardPage.GoToCart();
            productsAvaliability = _cartPage.ProductsAvailableInCart();
            Assert.That(productsAvaliability, Is.True, $"Products available in the cart.");
            Assert.That(_cartPage.CartItems.Count, Is.EqualTo(1), "Cart should contain one item after adding an item.");

            var productInCart = _cartPage.ProductsInCart();
            Assert.That(productName, Is.EqualTo(productInCart), "Added product in Dashboard is displaying on Cart Page.");

        }

        [Test]
        public async Task CheckoutProduct_AndReturnToOrderReviewPageAsync()
        {

            bool productsAvaliability = _cartPage.ProductsAvailableInCart();
            Assert.That(productsAvaliability, Is.False, "There's No products available in Your Cart");
            Assert.That(_cartPage.CartItems.Count, Is.Zero, "No Products in Your Cart !");

            _dashboardPage = _cartPage.ContinueShopping();
            string productName = _dashboardPage.AddProductToCartByIndex(1);

            _cartPage = _dashboardPage.GoToCart();

            _test.Log(Status.Info, "Checking if Product item/items available in Cart page");
            productsAvaliability = _cartPage.ProductsAvailableInCart();

            if (productsAvaliability)
                _test.Log(Status.Pass, "User can navigate to Order Review page");
            else
                _test.Log(Status.Fail, "User cannot navigate to Order Review page");
            Assert.That(productsAvaliability, Is.True, "Products are not available in the cart.");

            _test.Log(Status.Info, "Proceeding to checkout");
            _orderReviewPage = _cartPage.ProceedToCheckout();

            _test.Log(Status.Info, "Checking if we're on the Order Review page");
            bool isAtOrderReview = _orderReviewPage.IsIn_OrderReviewPage();

            if (isAtOrderReview)
                _test.Log(Status.Pass, "Successfully navigated to Order Review page");
            else
                _test.Log(Status.Fail, "Failed to navigate to Order Review page");

            Assert.That(isAtOrderReview, Is.True);

        }
    }
}
