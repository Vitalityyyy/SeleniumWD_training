using OpenQA.Selenium;


namespace SeleniumWD3_PageObjects
{
    public class ProductPage
    {
        public By Size { get { return By.Name("options[Size]"); } }
        public By AddToCartButton { get { return By.Name("add_cart_product"); } }
        public By GoToCart { get { return By.LinkText("Checkout »"); } }
    }
}
